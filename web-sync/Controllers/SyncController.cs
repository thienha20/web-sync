using Microsoft.AspNetCore.Mvc;
using web_sync.Services;

namespace web_sync.Controllers
{
    [ApiController]
    [Route("api/sync")]
    public class SyncController : Controller
    {
        private readonly SyncService _syncService;
        public SyncController(SyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet("insert")]
        public async Task<IActionResult> Index()
        {
            await _syncService.InsertAll();
            return Ok(new { message = "ok" });
        }

        [HttpPost("update")]
        public async Task<IActionResult> post()
        {
            await _syncService.UpdateAll();
            return Ok(new { message = "ok" });
        }

        [HttpGet("delete")]
        public async Task<IActionResult> user()
        {
            await _syncService.DeleteAll();
            return Ok(new { message = "ok" });
        }

    }
}
