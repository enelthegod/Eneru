namespace Eneru.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }                    // decimal for money
        public string ImageUrl { get; set; } = string.Empty; // ./image
        public string Brand { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;        // instock?
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        public int CategoryId { get; set; }
        public Category? Category { get; set; }              // navigation
    }
}
