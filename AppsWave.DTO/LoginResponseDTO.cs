using System;
using System.Collections.Generic;
using System.Text;

namespace AppsWave.DTO;

public class LoginResponseDTO
{
    public UserDTO User { get; set; }
    public string Token { get; set; }
}
