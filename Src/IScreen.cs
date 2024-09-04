using System.Threading.Tasks;
using EWeLink.Cube.Api.Models;

namespace EWeLink.Cube.Api;

public interface IScreen
{
    Task<bool> SetBrightness(ScreenBrightnessMode mode, int? value = null);

    Task<bool> SetDisplay(bool autoEnabled, int? duration = null);
}