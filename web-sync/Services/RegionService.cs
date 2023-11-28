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
            LogCbRepository logCbRepository)
        {
            _regionObRepository = RegionObRepository;
            _regionCbRepository = RegionCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
        }

        public async Task<bool> SyncInsert()
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
                            RegionId = item?.RegionId,
                            RegionName = item?.RegionName
                        });
                        if (item?.RegionId != null)
                        {
                            string dataContent = item?.RegionId.ToString() ?? "";
                            if (dataContent != "")
                            {
                                await _fileLogService.writeFile("region-insert", dataContent);
                            }
                        }
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
                var param = new RegionDto() { Limit = limit, Offset = 0, RegionIds = insertParam.id?.Select(l => (int)l).ToArray() };
                var result = await _regionCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        _regionObRepository.ReplaceInto(new RegionObModel()
                        {
                            RegionId = item?.RegionId,
                            RegionName = item?.RegionName
                        });
                    }
                    param.Offset += limit;
                    result = await _regionCbRepository.GetAll(param);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SyncUpdate()
        {
            try
            {
                int limit = 1000;
                string content = await _fileLogService.readFile("region-update");
                var param = new LogDto() {
                    ObjectName = "region",
                    ObjectTypes = new[] { "update" },
                    Limit = limit,
                    Offset = 0
                };
                if (content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out int fromLogId);
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
                        int id = (int)item?.ObjectId;
                        var RegionData = await _regionCbRepository.GetById(id);
                        if (RegionData != null)
                        {
                            _regionObRepository.ReplaceInto(new RegionObModel()
                            {
                                RegionId = RegionData.RegionId,
                                RegionName = RegionData.RegionName
                            });
                            if (item?.LogId != null)
                            {
                                string dataContent = item?.LogId.ToString() ?? "";
                                if (dataContent != "")
                                {
                                    await _fileLogService.writeFile("region-update", dataContent);
                                }
                            }
                        }
                        
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

        public async Task<bool> SyncDelete()
        {
            try
            {
                int limit = 1000;
                string content = await _fileLogService.readFile("region-delete");
                var param = new LogDto()
                {
                    ObjectName = "region",
                    ObjectTypes = new[] { "delete" },
                    Limit = limit,
                    Offset = 0
                };
                if (content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out int fromLogId);
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
                        int id = (int)item?.ObjectId;
                        _regionObRepository.Delete(id);
                        if (item?.LogId != null)
                        {
                            string dataContent = item?.LogId.ToString() ?? "";
                            if (dataContent != "")
                            {
                                await _fileLogService.writeFile("region-delete", dataContent);
                            }
                        }
                        
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
        public async Task SyncAll()
        {
            await SyncInsert();
            await SyncUpdate();
            await SyncDelete();
        }
    }
}
