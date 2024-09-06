using System;
using System.Collections.Generic;
using EWeLink.Cube.Api.Models.Devices;

namespace EWeLink.Cube.Api;

public class DeviceCache : IDeviceCache
{
    private readonly Dictionary<string, string> serialToModel = new();
    private readonly Dictionary<string, ISubDevice> serialToDevice = new();
    
    public bool TryGetDevice(string serialNumber, out ISubDevice? device)
    {
        device = null;
        if (this.serialToDevice.TryGetValue(serialNumber, out var val))
        {
            device = val;
            return true;
        }

        return false;
    }
    
    public ISubDevice? GetDevice(string serialNumber)
    {
        if (this.serialToDevice.TryGetValue(serialNumber, out var device))
        {
            return device;
        }

        return null;
    }

    public bool DeleteDevice(string serialNumber)
    {
        bool result = false;
        lock (this.serialToDevice)
        {
            result = this.serialToDevice.Remove(serialNumber);
            this.serialToModel.Remove(serialNumber);
        }

        return result;
    }

    public IEnumerable<ISubDevice> UpdateCache(IEnumerable<ISubDevice> devices)
    {
        foreach (var device in devices)
        {
            UpdateCache(device);
        }

        return devices;
    }

    public ISubDevice UpdateCache(ISubDevice device)
    {
        lock (this.serialToDevice)
        {
            if (device is { SerialNumber: not null })
            {
                if (!this.serialToDevice.ContainsKey(device.SerialNumber))
                {
                    this.serialToDevice.Add(device.SerialNumber, device);
                    this.serialToModel.Add(device.SerialNumber, device.Model);
                }
                else
                {
                    this.serialToDevice[device.SerialNumber] = device;
                    this.serialToModel[device.SerialNumber] = device.Model;
                }
            }

            return device;
        }
    }
}