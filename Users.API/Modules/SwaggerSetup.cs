using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace Users.API.Modules
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services) 
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Users.API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer Scheme: 'Bearer [space] 12345abcdef'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            //Scheme = "oauth2",
                            //Name = "Bearer",
                            //In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }
    }
}