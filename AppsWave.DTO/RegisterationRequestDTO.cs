using System;
using System.Collections.Generic;
using System.Text;

namespace AppsWave.DTO;

public class RegisterationRequestDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
