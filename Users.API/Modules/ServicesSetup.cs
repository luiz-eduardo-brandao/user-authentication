using Users.API.Services;

namespace Users.API.Modules
{
    public static class ServicesSetup
    {
        public static IServiceCollection AddServicesSetup(this IServiceCollection services) 
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}