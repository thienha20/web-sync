using web_sync.Repositories.ob;
using web_sync.Repositories.cb;
using web_sync.Models.ob;
using web_sync.Dtos;
using web_sync.Dtos.SyncParam;
namespace web_sync.Services
{
    public class RegionService
    {
        private readonly RegionObRepository _regionObRepository;
        private readonly RegionCbRepository _regionCbRepository;
        private readonly FileLogService _fileLogService;
        private readonly LogCbRepository _logCbRepository;
        public RegionService(RegionObRepository RegionObRepository, 
            RegionCbRepository RegionCbRepository,
            FileLogService fileLogService,
            LogCbRepository logCbRepository
            )
        {
            _regionObRepository = RegionObRepository;
            _regionCbRepository = RegionCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
        }

        public async Task<bool> syncInsert()
        {
            try
            {
                int limit = 1000, fromRegionId;
                var param = new RegionDto() { Limit = limit, Offset = 0 };
                string content = await _fileLogService.readFile("region-insert");
                if(content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out fromRegionId);
                    if (isValidInt)
                    {
                        param.FromRegionId = fromRegionId;
                    }
                }
                var result = await _regionCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        _regionObRepository.ReplaceInto(new RegionObModel()
                        {
                            RegionId = item.RegionId,
                            RegionName = item.RegionName
                        });
                        await _fileLogService.writeFile("region-insert", item.RegionId.ToString() ?? "");
                    }
                    param.Offset = param.Offset + limit;
                    result = await _regionCbRepository.GetAll(param);
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
                var param = new RegionDto() { Limit = limit, Offset = 0, RegionIds = insertParam.id };
                var result = await _regionCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        _regionObRepository.ReplaceInto(new RegionObModel()
                        {
                            RegionId = item.RegionId,
                            RegionName = item.RegionName
                        });
                    }
                    param.Offset = param.Offset + limit;
                    result = await _regionCbRepository.GetAll(param);
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
                string content = await _fileLogService.readFile("region-query-log");
                var param = new LogDto() {
                    ObjectName = "region",
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
                            var RegionData = await _regionCbRepository.GetById(item.ObjectId ?? 0);
                            if(RegionData != null)
                            {
                                _regionObRepository.ReplaceInto(new RegionObModel()
                                {
                                    RegionId = RegionData.RegionId,
                                    RegionName = RegionData.RegionName
                                });

                            }
                            
                        } else
                        {
                            _regionObRepository.Delete(item.ObjectId ?? 0);
                        }
                        await _fileLogService.writeFile("region-query-log", item.LogId.ToString() ?? "");
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
