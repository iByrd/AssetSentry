using Microsoft.EntityFrameworkCore;

namespace AssetSentry.Models
{
    public class AssetSentryContext : DbContext
    {
        public AssetSentryContext(DbContextOptions<AssetSentryContext> options) : base(options) { }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<Action> Actions { get; set; }

        public DbSet<ObjectType> ObjectType { get; set; }

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

            modelBuilder.Entity<Action>().HasData(
                new Action { ActionId = "add", Name = "Add" },
                new Action { ActionId = "edit", Name = "Edit" },
                new Action { ActionId = "remove", Name = "Remove" },
                new Action { ActionId = "end", Name = "End" }
            );

            modelBuilder.Entity<ObjectType>().HasData(
                new ObjectType { ObjectTypeId = "status", Name = "Status" },
                new ObjectType { ObjectTypeId = "loan", Name = "Loan" },
                new ObjectType { ObjectTypeId = "account", Name = "Account" }
            );
        }
    }
}

