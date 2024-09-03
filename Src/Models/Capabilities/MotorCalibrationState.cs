using System.Runtime.Serialization;

namespace EWeLink.Cube.Api.Models.Capabilities;

public enum MotorCalibrationState
{
    [EnumMember(Value = "normal")]
    Normal,
        
    [EnumMember(Value = "calibration")]
    Calibration,
}