using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TraditionalRx.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext (DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public DbSet<Category> Category { get; set; }
        public DbSet<MedicineCategory> MedicineCategory { get; set; }
        public DbSet<Medicine> Medicine { get; set; }
    }
}
