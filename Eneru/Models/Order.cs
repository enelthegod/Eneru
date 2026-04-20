namespace Eneru.Models
{
    public enum OrderStatus
    {
        Pending,    
        Processing, 
        Shipped,    
        Delivered,  
        Cancelled   
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalPrice { get; set; }

        // key  — whose order
        public int UserId { get; set; }
        public User? User { get; set; }

        // list 
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}