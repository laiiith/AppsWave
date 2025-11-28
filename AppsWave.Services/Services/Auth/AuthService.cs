using AppsWave.DTO;
using AppsWave.Services.Data;
using Microsoft.AspNetCore.Identity;

namespace AppsWave.Services.Services.Auth;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppsWave.Entites.User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext db, UserManager<AppsWave.Entites.User> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<string> Register(RegisterationRequestDTO registerationRequestDTO)
    {
        var user = new AppsWave.Entites.User
        {
            Email = registerationRequestDTO.Email,
            UserName = registerationRequestDTO.Username,
            NormalizedEmail = registerationRequestDTO.Email.ToUpper(),
            FullName = registerationRequestDTO.FullName,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
            if (result.Succeeded)
            {
                var userToReturn = _db.Users.First(u=> u.UserName == registerationRequestDTO.Email);
                return string.Empty;
            }
            else
            {
                return result.Errors.FirstOrDefault().Description;
            }
        }
        catch (Exception) { }

        return "Error Encountered";
    }

    public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email == loginRequestDTO.Email);

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

        if (user == null || !isValid)
        {
            return new LoginResponseDTO() {  Token = string.Empty , User = null };  
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token =  _jwtTokenGenerator.GenerateToken(user,roles);

        var userDTO = new UserDTO() { Email = user.Email, FullName = user.FullName,Id = user.Id ,UserName = user.UserName};

        return new LoginResponseDTO
        {
            Token = token,
            User = userDTO,
        };
    }

    public async Task<bool> AssignRole(string email, string role)
    {
        var user = _db.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        if (user is not null)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
               await _roleManager.CreateAsync(new IdentityRole(role));
            }
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }
        return false;
    }
}
