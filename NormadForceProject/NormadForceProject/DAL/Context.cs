using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NormadForceProject.Models;

namespace NormadForceProject.DAL
{
    public class Context : IdentityDbContext<AppUser>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
    }
}
