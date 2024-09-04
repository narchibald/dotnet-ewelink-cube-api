using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;

namespace EWeLink.Cube.Api;

public interface IHardware
{
    Task<bool> Reboot();
    
    Task<bool> PlaySound(PlaySound sound);
    
    Task<bool> PlaySound(PlayBeep beep);
}