using Microsoft.AspNetCore.Mvc;
using web_sync.Services;

namespace web_sync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : Controller
    {
        private readonly PostService _postService;
        private readonly CountryService _countryService;
        private readonly RegionService _regionService;
        public SyncController(PostService postService, 
            CountryService countryService,
            RegionService regionService)
        {
            _postService = postService;
            _countryService = countryService;
            _regionService = regionService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Index()
        {
            var data = await _countryService.syncInsert();
            //var dataRegion = await _regionService.syncUpdateOrDelete();
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
