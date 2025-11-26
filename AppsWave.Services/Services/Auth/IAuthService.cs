namespace AppsWave.Services.Services.Auth;

public interface IAuthService
{
    Task<string> Register(AppsWave.DTO.RegisterationRequestDTO registerationRequestDTO);
    Task<AppsWave.DTO.LoginResponseDTO> Login(AppsWave.DTO.LoginRequestDTO loginRequestDTO);
    Task<bool> AssignRole(string email,string role);
}
