using System.Text;
using ApplicationSecretKeys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Product.API.Constants;
using Product.API.PolicyRequirements;

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

        public static void AddAuthenticationPolicies(this IServiceCollection services) 
        {
            services.AddSingleton<IAuthorizationHandler, BusinessHoursHandler>();
            services.AddAuthorization(options => 
            {
                options.AddPolicy(Policies.BusinessHours, policy =>
                    policy.Requirements.Add(new BusinessHoursRequirement()));
            });
        }
    }
}