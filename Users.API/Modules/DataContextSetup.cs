using ApplicationSecretKeys;
using Microsoft.EntityFrameworkCore;
using Users.API.Data;

namespace Users.API.Modules
{
    public static class DataContextSetup
    {
        public static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<IdentityDataContext>(options =>
                options.UseSqlServer(DataBaseAccessValues.ConnectionString)
            );

            return services;
        }
    }
}