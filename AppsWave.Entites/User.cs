using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppsWave.Entites;

public class User : IdentityUser
{
    [Required]
    public string FullName { get; set; }  
}
