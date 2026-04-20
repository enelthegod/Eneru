namespace Eneru.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // save hash ( not password ) 
        public bool IsAdmin { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // navigation
        public List<Order> Orders { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
    }
}