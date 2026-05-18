using Eneru.Models;
using Eneru.Repositories;

namespace Eneru.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orders;
        private readonly ICartService _cart;

        public OrderService(IOrderRepository orders, ICartService cart)
        {
            _orders = orders;
            _cart = cart;
        }

        public Task<List<Order>> GetUserOrdersAsync(int userId)
            => _orders.GetByUserAsync(userId);

        public Task<Order?> GetOrderAsync(int id, int userId)
            => _orders.GetByIdAsync(id, userId);

        public async Task<Order> PlaceOrderAsync(int userId, List<CartItem> cartItems)
        {
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                TotalPrice = cartItems.Sum(c => c.Product!.Price * c.Quantity),
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    PriceAtPurchase = c.Product!.Price
                }).ToList()
            };

            await _orders.CreateAsync(order);
            await _cart.ClearCartAsync(userId);
            return order;
        }

        public Task<List<Order>> GetAllOrdersAsync()
            => _orders.GetAllAsync();

        public Task UpdateStatusAsync(int orderId, OrderStatus status)
            => _orders.UpdateStatusAsync(orderId, status);
    }
}