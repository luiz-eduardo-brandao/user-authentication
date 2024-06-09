using System.Text;
using ApplicationSecretKeys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Product.API.Modules
{
    public static class AuthenticationSetup
    {
        public static void AddAuthenticationSetup(this IServiceCollection services) 
        {
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters 
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtSecretValidationParameters.Issuer,
                    ValidAudience = JwtSecretValidationParameters.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecretValidationParameters.SecretKey)),
                    ClockSkew = TimeSpan.Zero,
                };
            });
        }
    }
}