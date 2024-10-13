using Entity;
using Microsoft.EntityFrameworkCore;
using SuperOwnerModels;
using TFU_Resident_API.Entity;

namespace TFU_Resident_API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<OTPMail> OTPMails { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedData();
        }
    }
}
