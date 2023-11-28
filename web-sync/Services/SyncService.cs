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

        public async Task<bool> InsertAll()
        {
            try
            {
                //chay insert cac khong phu thuoc truoc
                await _regionService.SyncInsert();
                await _countryService.SyncInsert();
                await _categoryService.SyncInsert();
                await _userService.SyncInsert();
                await _postService.SyncInsert();
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAll()
        {
            try
            {
                //chay insert cac khong phu thuoc truoc
                await _regionService.SyncUpdate();
                await _countryService.SyncUpdate();
                await _categoryService.SyncUpdate();
                await _userService.SyncUpdate();
                await _postService.SyncUpdate();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAll()
        {
            try
            {
                //chay insert cac khong phu thuoc truoc
                await _regionService.SyncDelete();
                await _countryService.SyncDelete();
                await _categoryService.SyncDelete();
                await _userService.SyncDelete();
                await _postService.SyncDelete();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
