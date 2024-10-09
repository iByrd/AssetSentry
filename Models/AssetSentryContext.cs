using Microsoft.EntityFrameworkCore;

namespace AssetSentry.Models
{
    public class AssetSentryContext : DbContext
    {
        public AssetSentryContext(DbContextOptions<AssetSentryContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>().HasData(
                new Device
                {
                    Id = 1,
                    Name = "TestDevice",
                    Description = "The old laptop found in a corner",
                    StatusId = "overdue",
                }
                );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "available", Name = "Available" },
                new Status { StatusId = "rented", Name = "Rented" },
                new Status { StatusId = "overdue", Name = "Overdue" }
            );
        }
    }
}

