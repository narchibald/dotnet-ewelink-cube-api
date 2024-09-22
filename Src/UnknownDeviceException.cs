using System;

namespace EWeLink.Cube.Api;

public class UnknownDeviceException(string serialNumber) : Exception
{
    public string SerialNumber { get; } = serialNumber;
}