using Entity;
using Microsoft.EntityFrameworkCore;

namespace TFU_Resident_API.Data
{
    public static class ResidentStore
    {
        public static void SeedData(this ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = Guid.Parse("98AE41E1-3379-4193-9856-1C9162A8C9C2"),
                    Name = "User",
                    IsActive = true,
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new Role
                {
                    Id = Guid.Parse("7FE6BD0C-AFE5-489D-982D-6F107F1D06FD"),
                    Name = "Admin",
                    IsActive = true,
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                }
                );

        }
    }
}
