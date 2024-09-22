using EWeLink.Cube.Api.Models.States;

namespace EWeLink.Cube.Api.Models.Devices;
    
[SubDeviceIdentifier("TX3C", Protocol = null)]
public class Tx3c : SubDevice<ThreeStateToggle>
{
}