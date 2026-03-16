using Simplified.Medistock.Models.ViewModels;

namespace Simplified.Medistock.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}