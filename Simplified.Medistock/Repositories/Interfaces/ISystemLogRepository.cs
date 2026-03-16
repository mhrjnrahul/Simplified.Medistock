using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Interfaces
{
    public interface ISystemLogRepository
    {
        Task<IEnumerable<SystemLog>> GetAllAsync();
        Task<SystemLog?> GetByIdAsync(int id);
        Task AddAsync(SystemLog log);
    }
}