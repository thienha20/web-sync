using Microsoft.AspNetCore.Mvc;
using sync_data.Services;

namespace sync_data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : Controller
    {
        private readonly PostService _postService;
        private readonly CountryService _countryService;
        public SyncController(PostService postService, CountryService countryService)
        {
            _postService = postService;
            _countryService = countryService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Index()
        {
            var data = await _countryService.syncCountry();
            return Ok(data);
        }

        [HttpGet("post")]
        public IActionResult post()
        {
            return View();
        }

        [HttpGet("user")]
        public IActionResult user()
        {
            return View();
        }

        [HttpGet("country")]
        public IActionResult country()
        {
            return View();
        }

        [HttpGet("region")]
        public IActionResult region()
        {
            return View();
        }

        [HttpGet("category")]
        public IActionResult category()
        {
            return View();
        }
    }
}
