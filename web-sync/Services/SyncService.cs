using web_sync.Repositories.ob;
using web_sync.Repositories.cb;
using web_sync.Models.ob;
using web_sync.Dtos;
using web_sync.Dtos.SyncParam;
namespace web_sync.Services
{
    public class SyncService
    {
        private readonly CategoryService _categoryService;
        private readonly PostService _postService;
        private readonly UserService _userService;
        private readonly CountryService _countryService;
        private readonly RegionService _regionService;
        public SyncService(CategoryService categoryService,
            PostService postService,
            UserService userService,
            CountryService countryService,
            RegionService regionService
        )
        {
            _categoryService = categoryService;
            _countryService = countryService;
            _postService = postService;
            _userService = userService;
            _regionService = regionService; 
        }

        public async Task<bool> BeginALl()
        {
            try
            {
                //chay insert cac khong phu thuoc truoc

                //chay cac update va delete

                return true;
            } catch
            {

                return false;
            }
        }
    }
}
