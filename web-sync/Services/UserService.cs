using web_sync.Repositories.ob;
using web_sync.Repositories.cb;
using web_sync.Models.ob;
using web_sync.Dtos;
using web_sync.Dtos.SyncParam;
namespace web_sync.Services
{
    public class UserService
    {
        private readonly UserObRepository _userObRepository;
        private readonly UserCbRepository _userCbRepository;
        private readonly FileLogService _fileLogService;
        private readonly LogCbRepository _logCbRepository;
        private readonly CountryService _countryService;
        public UserService(UserObRepository UserObRepository, 
            UserCbRepository UserCbRepository,
            FileLogService fileLogService,
            LogCbRepository logCbRepository, 
            CountryService countryService)
        {
            _userObRepository = UserObRepository;
            _userCbRepository = UserCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
            _countryService = countryService;
        }

        public async Task<bool> SyncInsert()
        {
            try
            {
                int limit = 1000;
                var param = new UserDto() { Limit = limit, Offset = 0 };
                string content = await _fileLogService.readFile("user-insert");
                if(content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out int fromUserId);
                    if (isValidInt)
                    {
                        param.FromUserId = fromUserId;
                    }
                }
                var result = await _userCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        bool error = false;
                        int timeInsertCountry = 0;
                        while (!error)
                        {
                            try
                            {
                                if (timeInsertCountry > 1) break; //trường hợp khóa không còn tồn tại trong db
                                _userObRepository.ReplaceInto(new UserObModel()
                                {
                                    UserId = item?.UserId,
                                    Email = item?.Email,
                                    UserName = item?.UserName,
                                    FullName = item?.FullName,
                                    CountryId = item?.CountryId,
                                    CreatedAt = item?.CreatedAt,
                                    UpdatedAt = item?.UpdatedAt
                                });
                                if (item?.UserId != null)
                                {
                                    string dataContent = item?.UserId.ToString() ?? "";
                                    if (dataContent != "")
                                    {
                                        await _fileLogService.writeFile("user-insert", dataContent);
                                    }
                                }
                                error = true;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("fk_country_id"))
                                {
                                    long[] ids = { item?.CountryId ?? 0 };
                                    await _countryService.SyncInsertWithCondition(new InsertDto() { id = ids });
                                    timeInsertCountry++;
                                }
                            }
                        }
                    }
                    param.Offset += limit;
                    result = await _userCbRepository.GetAll(param);
                }
                return true;
            } catch
            {
                return false;
            }
        }

        public async Task<bool> SyncInsertWithCondition(InsertDto insertParam)
        {
            try
            {
                int limit = 1000;
                var param = new UserDto() { 
                    Limit = limit, 
                    Offset = 0, 
                    UserIds = insertParam.id?.Select(l => (int)l).ToArray() 
                };
                var result = await _userCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        bool error = false;
                        int timeInsertCountry = 0;
                        while (!error)
                        {
                            try
                            {
                                if (timeInsertCountry > 1) break; //trường hợp khóa không còn tồn tại trong db
                                _userObRepository.ReplaceInto(new UserObModel()
                                {
                                    UserId = item?.UserId,
                                    Email = item?.Email,
                                    UserName = item?.UserName,
                                    FullName = item?.FullName,
                                    CountryId = item?.CountryId,
                                    CreatedAt = item?.CreatedAt,
                                    UpdatedAt = item?.UpdatedAt
                                });
                                error = true;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("fk_country_id"))
                                {
                                    long[] ids = { item?.CountryId ?? 0 };
                                    await _countryService.SyncInsertWithCondition(new InsertDto() { id = ids });
                                    timeInsertCountry++;
                                }
                            }
                        }
                        
                    }
                    param.Offset += limit;
                    result = await _userCbRepository.GetAll(param);
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
                var param = new UserDto() { Limit = limit, Offset = 0, IsUpdate = true, SortBy = "updated_at" };
                string content = await _fileLogService.readFile("user-update");
                if (content != null && content != "")
                {
                    param.UpdatedDateFrom = DateTime.Parse(content);
                }
                var result = await _userCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        if(item != null)
                        {
                            _userObRepository.ReplaceInto(new UserObModel()
                            {
                                UserId = item?.UserId,
                                Email = item?.Email,
                                UserName = item?.UserName,
                                FullName = item?.FullName,
                                CountryId = item?.CountryId,
                                CreatedAt = item?.CreatedAt,
                                UpdatedAt = item?.UpdatedAt
                            });
                            if (item?.UserId != null)
                            {
                                string dataContent = item?.UserId.ToString() ?? "";
                                if (dataContent != "")
                                {
                                    await _fileLogService.writeFile("user-update", dataContent);
                                }
                            }
                        }
                    }
                    param.Offset += limit;
                    result = await _userCbRepository.GetAll(param);
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
                string content = await _fileLogService.readFile("user-delete");
                var param = new LogDto()
                {
                    ObjectName = "user",
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
                        if (item?.ObjectId != null)
                        {
                            int id = (int)item?.ObjectId;
                            _userObRepository.Delete(id);

                            if (item?.LogId != null && item?.LogId.ToString() != "")
                            {
                                string dataContent = item?.LogId.ToString() ?? "";
                                if (dataContent != "")
                                {
                                    await _fileLogService.writeFile("user-delete", dataContent);
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
        public async Task SyncAll()
        {
            await SyncInsert();
            await SyncUpdate();
            await SyncDelete();
        }
    }
}
