using Microsoft.AspNetCore.Identity;

namespace AppsWave.Entites;

public class User : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
