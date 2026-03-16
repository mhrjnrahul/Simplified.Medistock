using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class SystemLogService : ISystemLogService
    {
        private readonly ISystemLogRepository _systemLogRepository;

        public SystemLogService(ISystemLogRepository systemLogRepository)
        {
            _systemLogRepository = systemLogRepository;
        }

        private static SystemLogViewModel MapToViewModel(SystemLog log) => new()
        {
            Id = log.Id,
            Timestamp = log.Timestamp,
            Level = log.Level,
            Message = log.Message,
            Exception = log.Exception,
            StackTrace = log.StackTrace,
            UserId = log.UserId,
            Action = log.Action,
            Controller = log.Controller,
            IpAddress = log.IpAddress
        };

        public async Task<IEnumerable<SystemLogViewModel>> GetAllAsync()
        {
            var logs = await _systemLogRepository.GetAllAsync();
            return logs.Select(MapToViewModel);
        }

        public async Task<SystemLogViewModel?> GetByIdAsync(int id)
        {
            var log = await _systemLogRepository.GetByIdAsync(id);
            if (log == null) return null;
            return MapToViewModel(log);
        }

        public async Task LogAsync(string message, AppLogLevel level, string? exception = null, string? controller = null, string? action = null)
        {
            await _systemLogRepository.AddAsync(new SystemLog
            {
                Message = message,
                Level = level,
                Exception = exception,
                Controller = controller,
                Action = action,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}