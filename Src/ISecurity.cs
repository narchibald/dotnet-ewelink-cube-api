namespace EWeLink.Cube.Api;

using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public interface ISecurity
{
    Task<IReadOnlyList<SecurityMode>> GetModes();
    
    Task<bool> Enable();
    
    Task<bool> Disable();
    
    Task<bool> EnableMode(int sid);
    
    Task<bool> DisableMode(int sid);
}