using System.Collections.Generic;
using EWeLink.Cube.Api.Models.Devices;

namespace EWeLink.Cube.Api;

public interface IDeviceCache
{
    bool TryGetDevice(string serialNumber, out ISubDevice? device);
    
    ISubDevice? GetDevice(string serialNumber);
    
    bool DeleteDevice(string serialNumber);
    
    IEnumerable<ISubDevice> UpdateCache(IEnumerable<ISubDevice> devices);
    
    ISubDevice UpdateCache(ISubDevice device);
}