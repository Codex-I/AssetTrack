using AssetTrackApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetTrackApi.Data
{
    public class AssetTrackContext(DbContextOptions<AssetTrackContext> options) : DbContext(options)
    {
        public DbSet<Asset> Assets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SubLocation> SubLocations { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<ConditionHistory> AssetsConditionHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // One-to-Many relationship between User and Asset (Owner)
            modelBuilder.Entity<User>()
                .HasMany(u => u.ManagedAssets)
                .WithOne()
                .HasForeignKey(a => a.Owner);

            // Configure the one-to-one relationship between Category and SubCategory
            modelBuilder.Entity<Category>()
                .HasOne(c => c.SubCategory)
                .WithOne()
                .HasForeignKey<SubCategory>(sc => sc.CategoryId);

            // disable cascade deletes on all tables
            foreach (var entity in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                entity.DeleteBehavior = DeleteBehavior.NoAction;
            }

            base.OnModelCreating(modelBuilder);
        }
    }

}
