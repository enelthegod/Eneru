using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eneru.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { "Essential everyday white t-shirt made from 100% organic cotton.", "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=600" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "ImageUrl" },
                values: new object[] { new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Modern slim fit black denim with stretch comfort technology.", "https://images.unsplash.com/photo-1542272604-787c3835535d?w=600" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "ImageUrl" },
                values: new object[] { new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Clean minimalist leather sneakers. Versatile and timeless.", "https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { 4, "Eneru Basics", 1, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Relaxed fit hoodie in premium heavyweight fleece.", "https://images.unsplash.com/photo-1556821840-3a63f15732ce?w=600", true, "Oversized Hoodie", 89.99m },
                    { 5, "Eneru Street", 2, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Utility cargo pants with multiple pockets. Perfect for everyday wear.", "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?w=600", true, "Cargo Pants", 119.99m },
                    { 6, "Eneru Accessories", 4, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Full grain leather belt with brushed silver buckle.", "https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=600", true, "Leather Belt", 59.99m },
                    { 7, "Eneru Sport", 3, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Lightweight performance sneakers with responsive cushioning.", "https://images.unsplash.com/photo-1608231387042-66d1773070a5?w=600", true, "Running Sneakers", 159.99m },
                    { 8, "Eneru Accessories", 4, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), "Durable canvas tote with internal zip pocket.", "https://images.unsplash.com/photo-1544816155-12df9643f363?w=600", true, "Canvas Tote Bag", 45.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ImageUrl" },
                values: new object[] { "Essential everyday white t-shirt", "/images/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Description", "ImageUrl" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Modern slim fit black denim", "/images/placeholder.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Description", "ImageUrl" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Clean minimalist leather sneakers", "/images/placeholder.jpg" });
        }
    }
}
