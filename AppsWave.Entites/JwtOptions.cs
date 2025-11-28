namespace AppsWave.Entites;

public class JwtOptions
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}

public static class Roles
{
    public const string ADMIN = "ADMIN";
    public const string VISITOR = "VISITOR";
}