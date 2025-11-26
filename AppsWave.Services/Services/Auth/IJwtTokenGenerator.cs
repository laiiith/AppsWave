namespace AppsWave.Services.Services.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(AppsWave.Entites.User user,IEnumerable<string> roles);
}
