using EWeLink.Cube.Api.Models.States;

namespace EWeLink.Cube.Api.Models;

[SubDeviceIdentifier("", DisplayCategory = "camera", Protocol = "rtsp")]
public class CameraDevice : SubDevice<CameraStreamState>
{
}