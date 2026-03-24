using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HyMall_App.Models;

namespace HyMall_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Fix non-nullable property warning by making it nullable
        public DbSet<ContactMessage> ContactMessages { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<TenantProfile> TenantProfiles { get; set; } 
    }
}
