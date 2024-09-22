using System.Net;

namespace EWeLink.Cube.Api
{
    public interface ILinkFactory
    {
        ILink Create(string ipAddress, string? accessToken = null, int? port = null,  ApiVersion? apiVersion = null);
        ILink Create(IPAddress ipAddress, string? accessToken = null, int? port = null,  ApiVersion? apiVersion = null);
    }
}