using Eneru.Models;

namespace Eneru.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task<Order?> GetOrderAsync(int id, int userId);
        Task<Order> PlaceOrderAsync(int userId, List<CartItem> cartItems);
        Task<List<Order>> GetAllOrdersAsync();
        Task UpdateStatusAsync(int orderId, OrderStatus status);
    }
}