using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<MenuItem> MenuItems => Set<MenuItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MenuItem configuration
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).HasConversion<string>();
                entity.Property(e => e.Price).HasConversion(
                    v => v.Amount,
                    v => new Money(v));
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.HasQueryFilter(e => e.IsActive);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).HasMaxLength(100);
                entity.Property(e => e.Status).HasConversion<string>();
                entity.Property(e => e.Subtotal).HasConversion(
                    v => v.Amount,
                    v => new Money(v));
                entity.Property(e => e.DiscountPercentage).HasPrecision(5, 4);
                entity.Property(e => e.DiscountAmount).HasConversion(
                    v => v.Amount,
                    v => new Money(v));
                entity.Property(e => e.Total).HasConversion(
                    v => v.Amount,
                    v => new Money(v));
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.HasQueryFilter(e => !e.IsDeleted);
                entity.HasMany(e => e.Items)
                    .WithOne(e => e.Order)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).HasConversion<string>();
                entity.Property(e => e.UnitPrice).HasConversion(
                    v => v.Amount,
                    v => new Money(v));
                entity.Property(e => e.Quantity).HasDefaultValue(1);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m) { IsActive = true },
                new MenuItem(2, "X Egg", ItemType.Sandwich, 4.50m) { IsActive = true },
                new MenuItem(3, "X Bacon", ItemType.Sandwich, 7.00m) { IsActive = true },
                new MenuItem(4, "Batata frita", ItemType.Side, 2.00m) { IsActive = true },
                new MenuItem(5, "Refrigerante", ItemType.Drink, 2.50m) { IsActive = true }
            );
        }
    }
}
