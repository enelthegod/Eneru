using Eneru.Models;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Each DbSet = one table in the database
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Decimal precision for all money fields
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.PriceAtPurchase)
                .HasColumnType("decimal(18,2)");

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Tops", Slug = "tops" },
                new Category { Id = 2, Name = "Bottoms", Slug = "bottoms" },
                new Category { Id = 3, Name = "Shoes", Slug = "shoes" },
                new Category { Id = 4, Name = "Accessories", Slug = "accessories" }
            );

            // Seed products with real Unsplash image URLs
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Classic White Tee",
                    Description = "Essential everyday white t-shirt made from 100% organic cotton.",
                    Price = 49.99m,
                    Brand = "Eneru Basics",
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 2,
                    Name = "Slim Black Jeans",
                    Description = "Modern slim fit black denim with stretch comfort technology.",
                    Price = 129.99m,
                    Brand = "Eneru Denim",
                    CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1542272604-787c3835535d?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 3,
                    Name = "White Leather Sneakers",
                    Description = "Clean minimalist leather sneakers. Versatile and timeless.",
                    Price = 189.99m,
                    Brand = "Eneru Sport",
                    CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 4,
                    Name = "Oversized Hoodie",
                    Description = "Relaxed fit hoodie in premium heavyweight fleece.",
                    Price = 89.99m,
                    Brand = "Eneru Basics",
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 4, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 5,
                    Name = "Cargo Pants",
                    Description = "Utility cargo pants with multiple pockets. Perfect for everyday wear.",
                    Price = 119.99m,
                    Brand = "Eneru Street",
                    CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 6,
                    Name = "Leather Belt",
                    Description = "Full grain leather belt with brushed silver buckle.",
                    Price = 59.99m,
                    Brand = "Eneru Accessories",
                    CategoryId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 6, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 7,
                    Name = "Running Sneakers",
                    Description = "Lightweight performance sneakers with responsive cushioning.",
                    Price = 159.99m,
                    Brand = "Eneru Sport",
                    CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 7, 0, 0, 0, DateTimeKind.Utc)
                },
                new Product
                {
                    Id = 8,
                    Name = "Canvas Tote Bag",
                    Description = "Durable canvas tote with internal zip pocket.",
                    Price = 45.99m,
                    Brand = "Eneru Accessories",
                    CategoryId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1544816155-12df9643f363?w=600",
                    IsAvailable = true,
                    CreatedAt = new DateTime(2024, 1, 8, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}