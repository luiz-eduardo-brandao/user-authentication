using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Users.API.Data
{
    public class IdentityDataContext : IdentityDbContext
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options) {}
    }
}