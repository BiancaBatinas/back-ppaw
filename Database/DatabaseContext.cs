using Microsoft.EntityFrameworkCore;

using ppawproject.Database.Entities;

namespace ppawproject.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Marketplace> Marketplaces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
          .HasOne(u => u.Subscription)
          .WithMany(s => s.Users)
          .HasForeignKey(s => s.SubscriptionId);

            modelBuilder.Entity<User>()
        .HasMany(u => u.Marketplaces)
        .WithOne(m => m.User)
        .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<User>()
         .HasMany(u => u.Products)
          .WithOne(p => p.User)
        .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Product>()
           .HasOne(u => u.User)
           .WithMany(p => p.Products)
           .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Product>()
            .HasOne(u => u.ParentProduct)
            .WithMany(p => p.Variants)
            .HasForeignKey(p => p.Id);

            modelBuilder.Entity<Product>()
            .HasMany(u => u.Variants)
            .WithOne(p => p.ParentProduct)
            .HasForeignKey(p => p.ParentProductId);

            modelBuilder.Entity<Product>()
            .HasOne(u => u.Marketplace)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.MarketplaceId);

            modelBuilder.Entity<Subscription>()
           .HasMany(u => u.Users)
           .WithOne(p => p.Subscription)
           .HasForeignKey(p => p.SubscriptionId);

            modelBuilder.Entity<Marketplace>()
         .HasMany(u => u.Products)
         .WithOne(p => p.Marketplace)
         .HasForeignKey(p => p.MarketplaceId);

            modelBuilder.Entity<Marketplace>()
            .HasOne(u => u.User)
            .WithMany(p => p.Marketplaces)
            .HasForeignKey(p => p.UserId);
        }

    }
}
