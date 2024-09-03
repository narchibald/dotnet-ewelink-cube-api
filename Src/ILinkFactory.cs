using System.Net;

namespace EWeLink.Cube.Api
{
    public interface ILinkFactory
    {
        ILink Create(string ipAddress, string? accessToken = null);
        ILink Create(IPAddress ipAddress, string? accessToken = null);
    }
}