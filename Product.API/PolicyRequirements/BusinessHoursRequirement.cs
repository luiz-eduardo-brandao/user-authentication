using Microsoft.AspNetCore.Authorization;

namespace Product.API.PolicyRequirements
{
    public class BusinessHoursRequirement : IAuthorizationRequirement
    {
        public BusinessHoursRequirement() { }
    }
}