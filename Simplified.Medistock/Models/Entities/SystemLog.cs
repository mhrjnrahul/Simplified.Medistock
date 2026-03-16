
using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.Entities
{
    // Stores important app events — logins, deletions, errors, etc.
    public class SystemLog
    {
        // No BaseEntity here — logs are never soft deleted or updated
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public AppLogLevel Level { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public string? UserId { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public string? IpAddress { get; set; }
    }
}
