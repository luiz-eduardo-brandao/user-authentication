using Microsoft.AspNetCore.Identity;

namespace Users.Tests.Fixtures
{
    public static class UserFixtures
    {
        public static IdentityUser User = new IdentityUser
        {
            Email = "test@gmail.com"
        };
    }
}
