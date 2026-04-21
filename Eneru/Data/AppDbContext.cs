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
            // Decimal precision for money fields (important for prices)
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.PriceAtPurchase)
                .HasColumnType("decimal(18,2)");



            // Seed data — starter categories so the site isn't empty
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Tops", Slug = "tops" },
                new Category { Id = 2, Name = "Bottoms", Slug = "bottoms" },
                new Category { Id = 3, Name = "Shoes", Slug = "shoes" },
                new Category { Id = 4, Name = "Accessories", Slug = "accessories" }
            );



            // Seed data — starter products
            modelBuilder.Entity<Product>().HasData(

     new Product                            // m after price = just to identify that is decimal
     {
         Id = 1,
         Name = "Classic White Tee",
         Description = "Essential everyday white t-shirt",
         Price = 49.99m,
         Brand = "Eneru Basics",
         CategoryId = 1,
         ImageUrl = "/images/placeholder.jpg",
         CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // static date
     },
     new Product
     {
         Id = 2,
         Name = "Slim Black Jeans",
         Description = "Modern slim fit black denim",
         Price = 129.99m,
         Brand = "Eneru Denim",
         CategoryId = 2,
         ImageUrl = "/images/placeholder.jpg",
         CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // static date
     },
     new Product
     {
         Id = 3,
         Name = "White Leather Sneakers",
         Description = "Clean minimalist leather sneakers",
         Price = 189.99m,
         Brand = "Eneru Sport",
         CategoryId = 3,
         ImageUrl = "/images/placeholder.jpg",
         CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // static date
     }

            );
        }
    }
}