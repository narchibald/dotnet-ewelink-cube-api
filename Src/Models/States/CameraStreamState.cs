using System.Collections.Generic;
using EWeLink.Cube.Api.Models.Capabilities;
using Newtonsoft.Json;

namespace EWeLink.Cube.Api.Models.States
{
    public class CameraStreamState : SubDeviceState
    {
        [JsonProperty("camera-stream")]
        public CameraStreamCapability CameraStream { get; set; }
    }
}