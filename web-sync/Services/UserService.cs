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
        public UserService(UserObRepository UserObRepository, 
            UserCbRepository UserCbRepository,
            FileLogService fileLogService,
            LogCbRepository logCbRepository
            )
        {
            _userObRepository = UserObRepository;
            _userCbRepository = UserCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
        }

        public async Task<bool> syncInsert()
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
                        await _fileLogService.writeFile("user-insert", item?.UserId.ToString() ?? "");
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

        public async Task<bool> syncInsertWithCondition(InsertDto insertParam)
        {
            try
            {
                int limit = 1000;
                var param = new UserDto() { Limit = limit, Offset = 0, UserIds = insertParam.id };
                var result = await _userCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
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
        public async Task<bool> syncUpdateOrDelete()
        {
            try
            {
                int limit = 1000;
                string content = await _fileLogService.readFile("user-query-log");
                var param = new LogDto() {
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
                        if(item?.ObjectType == "update")
                        {
                            var UserData = await _userCbRepository.GetById(item.ObjectId ?? 0);
                            if(UserData != null)
                            {
                                _userObRepository.ReplaceInto(new UserObModel()
                                {
                                    UserId = UserData?.UserId,
                                    Email = UserData?.Email,
                                    UserName = UserData?.UserName,
                                    FullName = UserData?.FullName,
                                    CountryId = UserData?.CountryId,
                                    CreatedAt = UserData?.CreatedAt,
                                    UpdatedAt = UserData?.UpdatedAt
                                });

                            }
                            
                        } else
                        {
                            _userObRepository.Delete(item?.ObjectId ?? 0);
                        }
                        await _fileLogService.writeFile("user-query-log", item?.LogId?.ToString() ?? "");
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
            bool bol = await syncInsert();
            if (!bol)
            {
                return false;
            }
            bol = await syncUpdateOrDelete();
            return bol;
        }
    }
}
