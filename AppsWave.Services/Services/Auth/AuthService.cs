using AppsWave.DTO.Auth;
using AppsWave.Entites;
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
            EmailConfirmed = true
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
            if (result.Succeeded)
            {
                var userToReturn = _db.Users.First(u => u.Email == registerationRequestDTO.Email);

                var roleResult = await AssignRole(registerationRequestDTO.Email, Roles.VISITOR);

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
            return new LoginResponseDTO() { Token = string.Empty, User = null };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        var userDTO = new UserDTO() { Email = user.Email, FullName = user.FullName, Id = user.Id, UserName = user.UserName };

        return new LoginResponseDTO
        {
            Token = token,
            User = userDTO,
        };
    }

    private async Task<bool> AssignRole(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        if (!await _roleManager.RoleExistsAsync(roleName))
            await _roleManager.CreateAsync(new IdentityRole(roleName));

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }
}
