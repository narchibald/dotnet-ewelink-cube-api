using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;

namespace EWeLink.Cube.Api;

public interface IGateway
{
    Task<Status> GetStatus();
    
    Task<Info> GetInfo();
    
    Task<bool> SetConfig(GatewayConfig config);
}