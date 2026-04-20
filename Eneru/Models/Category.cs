namespace Eneru.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;     // "Shoes", "Tops"
        public string Slug { get; set; } = string.Empty;     // "shoes" — for URL /products/shoes

        // Navigation — list staff from category
        public List<Product> Products { get; set; } = new();
    }
}
