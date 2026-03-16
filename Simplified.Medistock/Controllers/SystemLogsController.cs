using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Controllers
{
    [Authorize]
    public class SystemLogsController : Controller
    {
        private readonly ISystemLogService _systemLogService;

        public SystemLogsController(ISystemLogService systemLogService)
        {
            _systemLogService = systemLogService;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _systemLogService.GetAllAsync();
            return View(logs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var log = await _systemLogService.GetByIdAsync(id);
            if (log == null) return NotFound();
            return View(log);
        }
    }
}