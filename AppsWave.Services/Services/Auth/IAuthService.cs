using AppsWave.DTO.Auth;

namespace AppsWave.Services.Services.Auth;

public interface IAuthService
{
    Task<string> Register(RegisterationRequestDTO registerationRequestDTO);
    Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
}
