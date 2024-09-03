using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;

namespace EWeLink.Cube.Api;

public interface ILink
{
    IPAddress IpAddress { get; }
    string? AccessToken { get; }
    Task<string?> GetAccessToken(CancellationToken? cancellationToken = default);
    Task<IReadOnlyList<ISubDevice>> GetDevices();
}