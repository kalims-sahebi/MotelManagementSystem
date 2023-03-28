using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MotelManagement.Models;

namespace MotelManagement.Data
{
    public class ApplicationDbContext: IdentityDbContext<MyAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // You can globally assign schema here
            builder.HasDefaultSchema("Users");

        }
    }
}
