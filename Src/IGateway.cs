using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;
using EWeLink.Cube.Api.Models.v1;

namespace EWeLink.Cube.Api;

public interface IGateway
{
    Task<Status> GetStatus();
    
    Task<Info> GetInfo();
    
    Task<bool> SetConfig(GatewayConfig config);
}