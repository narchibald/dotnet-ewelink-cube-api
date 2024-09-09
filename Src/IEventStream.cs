using System;
using EWeLink.Cube.Api.Models.States;

namespace EWeLink.Cube.Api;

public interface IEventStream
{
    event Action<ILinkEvent<SubDeviceState>>? StateUpdated;
}