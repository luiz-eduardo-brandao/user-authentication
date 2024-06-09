using Microsoft.AspNetCore.Identity;
using Users.API.Data;

namespace Users.API.Modules
{
    public static class IdentitySetup
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services) 
        {
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();
            
            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
            });

            return services;
        }
    }
}