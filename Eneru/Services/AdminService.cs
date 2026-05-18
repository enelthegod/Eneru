using Eneru.Data;
using Microsoft.EntityFrameworkCore;

namespace Eneru.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _db;

        public AdminService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(int products, int orders, int users)> GetDashboardStatsAsync()
        {
            var products = await _db.Products.CountAsync();
            var orders = await _db.Orders.CountAsync();
            var users = await _db.Users.CountAsync();
            return (products, orders, users);
        }
    }
}