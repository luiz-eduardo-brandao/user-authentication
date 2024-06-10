using Microsoft.AspNetCore.Authorization;

namespace Product.API.PolicyRequirements
{
    public class BusinessHoursHandler : AuthorizationHandler<BusinessHoursRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BusinessHoursRequirement requirement) 
        {
            var currentTime =TimeOnly.FromDateTime(DateTime.Now);

            if (currentTime.Hour >= 10 && currentTime.Hour <= 18)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}