namespace ApplicationSecretKeys;

public class JwtSecretValidationParameters
{
    public const string Issuer = "Users";
    public const string Audience = "UserAuthenticationAudience";
    public const string SecretKey = "abra#cadabra$sim@salabim*2023abra#cadabra$sim@salabim*2023abra#cadabra$sim@salabim*2023";
    public const int AccessTokenExpiration = 60;
    public const int RefreshTokenExpiration = 180;
}
