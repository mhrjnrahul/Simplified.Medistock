using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;

namespace Simplified.Medistock.Services.Interfaces
{
    public interface ISystemLogService
    {
        Task<IEnumerable<SystemLogViewModel>> GetAllAsync();
        Task<SystemLogViewModel?> GetByIdAsync(int id);
        Task LogAsync(string message, AppLogLevel level, string? exception = null, string? controller = null, string? action = null);
    }
}