using sync_data.Repositories.ob;
using sync_data.Repositories.cb;
using sync_data.Models.ob;
using sync_data.Models.cb;
using sync_data.Dtos;

namespace sync_data.Services
{
    public class CountryService
    {
        private readonly CountryObRepository _countryObRepository;
        private readonly CountryCbRepository _countryCbRepository;
        public CountryService(CountryObRepository countryObRepository , CountryCbRepository countryCbRepository)
        {
            _countryObRepository = countryObRepository;
            _countryCbRepository = countryCbRepository;
        }

        public async Task<Boolean> syncCountry()
        {
            var param = new CountryDto();
            var result = await _countryCbRepository.GetAll(param);
            foreach(var item in result)
            {
                _countryObRepository.ReplaceInto(new CountryObModel()
                {
                    CountryId = item.CountryId,
                    CountryName = item.CountryName,
                    CountryCode = item.CountryCode,
                    RegionId = item.RegionId,
                });
            }
            return true;
        }
    }
}
