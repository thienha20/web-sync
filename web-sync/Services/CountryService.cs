using web_sync.Repositories.ob;
using web_sync.Repositories.cb;
using web_sync.Models.ob;
using web_sync.Dtos;
using web_sync.Dtos.SyncParam;
namespace web_sync.Services
{
    public class CountryService
    {
        private readonly CountryObRepository _countryObRepository;
        private readonly CountryCbRepository _countryCbRepository;
        private readonly FileLogService _fileLogService;
        private readonly LogCbRepository _logCbRepository;

        private readonly RegionService _regionService;
        public CountryService(CountryObRepository countryObRepository, 
            CountryCbRepository countryCbRepository,
            FileLogService fileLogService,
            LogCbRepository logCbRepository,
            RegionService regionService)
        {
            _countryObRepository = countryObRepository;
            _countryCbRepository = countryCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
            _regionService = regionService;
        }

        public async Task<bool> syncInsert()
        {
            try
            {
                int limit = 1000, fromCountryId;
                var param = new CountryDto() { Limit = limit, Offset = 0 };
                string content = await _fileLogService.readFile("country-insert");
                if(content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out fromCountryId);
                    if (isValidInt)
                    {
                        param.FromCountryId = fromCountryId;
                    }
                }
                var result = await _countryCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        try
                        {
                            _countryObRepository.ReplaceInto(new CountryObModel()
                            {
                                CountryId = item.CountryId,
                                CountryName = item.CountryName,
                                CountryCode = item.CountryCode,
                                RegionId = item.RegionId,
                            });
                            await _fileLogService.writeFile("country-insert", item.CountryId.ToString() ?? "");
                        } catch (Exception ex)
                        {
                            // không add dc khi thiếu khóa ngoại thì add khóa
                            if (ex.Message.Contains("countries_region_id_fkey"))
                            {
                                await _regionService.syncInsertWithCondition(new InsertDto() { id = new long[] { item.RegionId ?? 0 } });
                                _countryObRepository.ReplaceInto(new CountryObModel()
                                {
                                    CountryId = item.CountryId,
                                    CountryName = item.CountryName,
                                    CountryCode = item.CountryCode,
                                    RegionId = item.RegionId,
                                });
                                await _fileLogService.writeFile("country-insert", item.CountryId.ToString() ?? "");
                                continue;
                            }
                        }
                        
                    }
                    param.Offset = param.Offset + limit;
                    result = await _countryCbRepository.GetAll(param);
                }
                return true;
            } catch
            {
                return false;
            }
        }
        public async Task<bool> syncInsertWithCondition(InsertDto insertParam)
        {
            try
            {
                int limit = 1000;
                var param = new CountryDto() { Limit = limit, Offset = 0, CountryIds = insertParam.id ?? null };             
                var result = await _countryCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        try
                        {
                            _countryObRepository.ReplaceInto(new CountryObModel()
                            {
                                CountryId = item.CountryId,
                                CountryName = item.CountryName,
                                CountryCode = item.CountryCode,
                                RegionId = item.RegionId,
                            });
                        }
                        catch (Exception ex)
                        {
                            // không add dc khi thiếu khóa ngoại thì add khóa
                            if (ex.Message.Contains("region_id_fk"))
                            {
                                await _regionService.syncInsertWithCondition(new InsertDto() { id = new long[] { item.RegionId ?? 0 } });
                                _countryObRepository.ReplaceInto(new CountryObModel()
                                {
                                    CountryId = item.CountryId,
                                    CountryName = item.CountryName,
                                    CountryCode = item.CountryCode,
                                    RegionId = item.RegionId,
                                });
                                continue;
                            }
                        }

                    }
                    param.Offset = param.Offset + limit;
                    result = await _countryCbRepository.GetAll(param);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> syncUpdateOrDelete()
        {
            try
            {
                int limit = 1000, fromLogId;
                string content = await _fileLogService.readFile("country-query-log");
                var param = new LogDto() {
                    ObjectName = "country",
                    ObjectTypes = new[] { "update", "delete" },
                    Limit = limit,
                    Offset = 0
                };
                if (content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out fromLogId);
                    if (isValidInt)
                    {
                        param.FromLogId = fromLogId;
                    }
                }
                var result = await _logCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        if(item.ObjectType == "update")
                        {
                            var countryData = await _countryCbRepository.GetById(item.ObjectId ?? 0);
                            if(countryData != null)
                            {
                                _countryObRepository.ReplaceInto(new CountryObModel()
                                {
                                    CountryId = countryData.CountryId,
                                    CountryName = countryData.CountryName,
                                    CountryCode = countryData.CountryCode,
                                    RegionId = countryData.RegionId,
                                });

                            }
                            
                        } else
                        {
                            _countryObRepository.Delete(item.ObjectId ?? 0);
                        }
                        await _fileLogService.writeFile("country-query-log", item.LogId.ToString() ?? "");
                    }
                    param.Offset += limit;
                    result = await _logCbRepository.GetAll(param);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> syncAll()
        {
            bool bol = false;
            bol = await syncInsert();
            bol = await syncUpdateOrDelete();
            return bol;
        }
    }
}
