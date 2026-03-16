using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.ViewModels
{
    public class SystemLogViewModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
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