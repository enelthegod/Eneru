using Eneru.Models;

namespace Eneru.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetByUserAsync(int userId);
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id, int userId);
        Task<Order> CreateAsync(Order order);
        Task UpdateStatusAsync(int orderId, OrderStatus status);
    }
}