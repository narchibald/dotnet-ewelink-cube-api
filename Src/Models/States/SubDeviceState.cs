using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EWeLink.Cube.Api.Models.States
{
    public class SubDeviceState
    {
        public int? Update(dynamic data)
        {
            return Update(JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                Converters = [new StringEnumConverter()],
                NullValueHandling = NullValueHandling.Ignore
            }));
        }

        public virtual int? Update(string jsonData)
        {
            JsonConvert.PopulateObject(jsonData, this);
            return null;
        }
    }
}