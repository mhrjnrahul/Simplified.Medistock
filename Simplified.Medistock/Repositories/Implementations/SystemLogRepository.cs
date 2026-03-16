using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Data;
using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;

namespace Simplified.Medistock.Repositories.Implementations
{
    public class SystemLogRepository : ISystemLogRepository
    {
        private readonly AppDbContext _context;

        public SystemLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SystemLog>> GetAllAsync()
        {
            return await _context.SystemLogs
                .ToListAsync();
        }

        public async Task<SystemLog?> GetByIdAsync(int id)
        {
            return await _context.SystemLogs.FindAsync(id);
        }

        public async Task AddAsync(SystemLog log)
        {
            await _context.SystemLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}