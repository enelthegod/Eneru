namespace Eneru.Services
{
    public interface IAdminService
    {
        Task<(int products, int orders, int users)> GetDashboardStatsAsync();
    }
}