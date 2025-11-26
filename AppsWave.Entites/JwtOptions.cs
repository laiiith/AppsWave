using System;
using System.Collections.Generic;
using System.Text;

namespace AppsWave.Entites;

public class JwtOptions
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
