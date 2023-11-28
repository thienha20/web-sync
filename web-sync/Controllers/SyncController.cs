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
        private readonly CategoryService _categoryService;
        private readonly UserService _userService;
        public SyncController(PostService postService, 
            CountryService countryService,
            RegionService regionService,
            CategoryService categoryService, 
            UserService userService)
        {
            _postService = postService;
            _countryService = countryService;
            _regionService = regionService;
            _categoryService = categoryService;
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> Index()
        {
           await _postService.SyncAll();
            //var dataRegion = await _regionService.syncUpdateOrDelete();
            return Ok(new { message = "ok" });
        }

        [HttpPost("post")]
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
