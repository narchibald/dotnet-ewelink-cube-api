using System;
using EWeLink.Cube.Api.Models.Capabilities;
using EWeLink.Cube.Api.Models.States;

namespace EWeLink.Cube.Api.Models.Devices;

[SubDeviceIdentifier("", DisplayCategory = "camera", Protocol = "rtsp")]
public class CameraDevice : SubDevice<CameraStreamState>
{
    public CameraDevice() {}

    public CameraDevice(string name, SubDeviceProtocol protocol, string streamUrl, string firmwareVersion = "", string model = "", string manufacturer = "")
    {
        if (protocol is not SubDeviceProtocol.Rtsp and not SubDeviceProtocol.Esp32Cam)
            throw new ArgumentOutOfRangeException(nameof(protocol), protocol, "Protocol must be either Rtsp or Esp32Cam.");
        this.Name = name;
        this.DisplayCategory = "camera";
        this.Protocol = protocol;
        this.Manufacturer = manufacturer;
        this.Model = model;
        this.FirmwareVersion = firmwareVersion;
        this.Capabilities =
        [
            new SubDeviceCapability
            {
                Capability = "camera-stream",
                Permission = Permission.Query,
                Configuration = new() { { "stream_url", streamUrl } }
            }
        ];
    }
}