using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Extensions;
using EWeLink.Cube.Api.Models.States;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EWeLink.Cube.Api;

public interface ILinkEvent<out T>
    where T : ISubDeviceState
{
    string SerialNumber { get; }
        
    string? ThirdPartySerialNumber { get; }
    
    T State { get; }
}

internal class EventStream(ILinkControl control, IHttpClientFactory httpClientFactory, ILogger<EventStream> logger)
{
	private enum EventType
	{
		None,
		[EnumMember(Value = "retry")]
		
		Retry,
		[EnumMember(Value = "data")]
		
		Data,
		[EnumMember(Value = "event")]
		
		Event,
		[EnumMember(Value = "id")]
		Id
	}
	
	private enum MessageType
	{
		Unknown,
		
		[EnumMember(Value = "addDevice")]
		AddDevice,
		
		[EnumMember(Value = "updateDeviceState")]
		UpdateDeviceState,
		
		[EnumMember(Value = "updateDeviceInfo")]
		UpdateDeviceInfo,
		
		[EnumMember(Value = "deleteDevice")]
		DeleteDevice,
		
		[EnumMember(Value = "updateDeviceOnline")]
		UpdateDeviceOnline
	}

    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    
    public event Action<ILinkEvent<ISubDeviceState>>? StateUpdated;

    public async Task Monitor()
    {
	    string accessToken = control.EnsureAccessToken();

	    var uri = new UriBuilder($"http://{control.IpAddress}:{control.Port}")
	    {
		    Path = "/open-api/v1/sse/bridge",
		    Query = "access_token=" + accessToken
	    };

	    var client = httpClientFactory.CreateClient();

	    int retryTime = 5;
	    while (!cancellationTokenSource.IsCancellationRequested)
	    {
		    try
		    {
			    logger.LogDebug("Establishing connection");
			    using (var streamReader = new StreamReader(await client.GetStreamAsync(uri.Uri)))
			    {
				    EventData? eventData = null;
				    StringBuilder dataBuffer = new();
				    while (!streamReader.EndOfStream)
				    {
					    var message = await streamReader.ReadLineAsync();
					    if (message is null)
						    continue;
					    
					    if (message.First() == '{' && message.Last() == '}')
					    {
						    var response = JsonConvert.DeserializeObject<Link.OpenApiResponse<Link.EmptyData>>(message);
						    if (response is not null && response.Error != 0)
						    {
							    throw new RequestException(response.Error, response.Message ?? "Unknown error");
						    }
					    }

					    var (eventType, content) = AnalyzeMessage(message);
					    if (eventType == EventType.Retry)
					    {
						    retryTime = int.Parse(content);
						    continue;
					    }
					    if (eventType == EventType.Event)
					    {
						    eventData = AnalyzeEvent(content);
						    dataBuffer.Clear();
						    continue;
					    }
					    if (eventType == EventType.Data)
					    {
						    dataBuffer.AppendLine(content);
						    continue;
					    }
					    if (eventType == EventType.Id)
					    {
						    continue;
					    }

					    if (eventData is null)
					    {
						    try
						    {
							    var json = JObject.Parse(dataBuffer.ToString());
							    // TODO handle event types
							    switch (eventData.MessageType)
							    {
								    case MessageType.UpdateDeviceState:
										await HandleStateUpdate(json);
									    break;
									default:
										break;
							    }
						    }
						    finally
						    {
							    eventData = null;
							    dataBuffer.Clear();
						    }
					    }
				    }
			    }
		    }
		    catch (Exception ex) when (ex is not RequestException)
		    {
			    logger.LogDebug("Error: {Message}", ex.Message);
			    logger.LogDebug("Retrying in {retryTime} milliseconds", retryTime);
			    await Task.Delay(TimeSpan.FromSeconds(retryTime));
		    }
	    }
    }

    private Task HandleStateUpdate(JObject json)
    {
	    var endPoint = json["endpoint"];
	    string serial = endPoint!["serial_number"]!.Value<string>()!;
	    
	    // look up device by serial to get its model to be able to serialize the payload into the state update

	    return Task.CompletedTask;
    }
    
    private (EventType eventType, string content) AnalyzeMessage(string message)
    {
	    StringBuilder type = new ();
	    StringBuilder content = new ();
	    bool typeFound = false;
	    foreach (var c in message)
	    {
		    if (!typeFound)
		    {
			    if (c != ':')
				    type.Append(c);
			    else
				    typeFound = true;
		    }
		    else
		    {
			    if (content.Length == 0 && c != ' ')
				    content.Append(c);
		    }
	    }

	    var messageType = type.ToString().ParseFromEnumMemberValue(EventType.None);
	    return (messageType, content.ToString());
    }
    
    private EventData AnalyzeEvent(string data)
    {
	    var parts = data.Split('#');

	    var messageType = parts.Last().ParseFromEnumMemberValue(MessageType.Unknown);
		
	    return new (parts[0], parts[1], messageType);
    }
    
    private class EventData(string moduleName, string version, MessageType messageType)
    {
	    public string ModuleName { get; } = moduleName;
	    
	    public string Version { get; } = version;
	    public MessageType MessageType { get; } = messageType;

	    public override string ToString()
	    {
		    return ModuleName + "#" + Version + "#" + MessageType.GetEnumMemberValue();
	    }
    }
}