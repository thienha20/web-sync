using web_sync.Repositories.ob;
using web_sync.Repositories.cb;
using web_sync.Models.ob;
using web_sync.Dtos;
using web_sync.Dtos.SyncParam;
namespace web_sync.Services
{
    public class PostService
    {
        private readonly PostObRepository _postObRepository;
        private readonly PostCbRepository _postCbRepository;
        private readonly FileLogService _fileLogService;
        private readonly LogCbRepository _logCbRepository;
        private readonly UserService _userService;
        private readonly CategoryService _categoryService;
        public PostService(PostObRepository PostObRepository,
            PostCbRepository PostCbRepository,
            FileLogService fileLogService,
            LogCbRepository logCbRepository,
            UserService userService,
            CategoryService categoryService
            )
        {
            _postObRepository = PostObRepository;
            _postCbRepository = PostCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
            _userService = userService;
            _categoryService = categoryService;
        }

        public async Task<bool> SyncInsert()
        {
            try
            {
                int limit = 1000;
                var param = new PostDto() { Limit = limit, Offset = 0 };
                string content = await _fileLogService.readFile("post-insert");
                if (content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out int fromPostId);
                    if (isValidInt)
                    {
                        param.FromPostId = fromPostId;
                    }
                }
                var result = await _postCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        bool error = false;
                        int timeInsertUser = 0;
                        int timeInsertCategory = 0;
                        while (!error)
                        {
                            try
                            {
                                if (timeInsertUser > 1 || timeInsertCategory > 1) break; //trường hợp khóa không còn tồn tại trong db
                                _postObRepository.ReplaceInto(new PostObModel()
                                {
                                    PostId = item?.PostId,
                                    Name = item?.Name,
                                    Description = item?.Description,
                                    UserId = item?.UserId,
                                    CategoryId = item?.CategoryId,
                                    CreatedAt = item?.CreatedAt,
                                    UpdatedAt = item?.UpdatedAt
                                });
                                error = true;
                                if (item?.PostId != null)
                                {
                                    string dataContent = item?.PostId.ToString() ?? "";
                                    if (dataContent != "")
                                    {
                                        await _fileLogService.writeFile("post-insert", dataContent);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("fk_user_id"))
                                {
                                    long[] ids = { item?.UserId ?? 0 };
                                    await _userService.SyncInsertWithCondition(new InsertDto() { id = ids });
                                    timeInsertUser++;
                                }
                                if (ex.Message.Contains("fk_category_id"))
                                {
                                    long[] ids = { item?.CategoryId ?? 0 };
                                    await _categoryService.SyncInsertWithCondition(new InsertDto() { id = ids });
                                    timeInsertCategory++;
                                }
                                continue;
                            }
                        }
                        
                    }
                    param.Offset += limit;
                    result = await _postCbRepository.GetAll(param);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SyncInsertWithCondition(InsertDto insertParam)
        {
            try
            {
                int limit = 1000;
                var param = new PostDto() { Limit = limit, Offset = 0, PostIds = insertParam.id?.Select(l => (int)l).ToArray() };
                var result = await _postCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        bool error = false;
                        int timeInsertUser = 0;
                        int timeInsertCategory = 0;
                        while (!error)
                        {
                            try
                            {
                                if (timeInsertUser > 1 || timeInsertCategory > 1) break; //trường hợp khóa không còn tồn tại trong db
                                _postObRepository.ReplaceInto(new PostObModel()
                                {
                                    PostId = item?.PostId,
                                    Name = item?.Name,
                                    Description = item?.Description,
                                    UserId = item?.UserId,
                                    CategoryId = item?.CategoryId,
                                    CreatedAt = item?.CreatedAt,
                                    UpdatedAt = item?.UpdatedAt
                                });
                                error = true;
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("fk_user_id"))
                                {
                                    long[] ids = { item?.UserId ?? 0 };
                                    await _userService.SyncInsertWithCondition(new InsertDto() { id = ids });
                                    timeInsertUser++;
                                }
                                if (ex.Message.Contains("fk_category_id"))
                                {
                                    long[] ids = { item?.CategoryId ?? 0 };
                                    await _categoryService.SyncInsertWithCondition(new InsertDto() { id = ids });
                                    timeInsertCategory++;
                                }
                                continue;
                            }
                        }
                    }
                    param.Offset += limit;
                    result = await _postCbRepository.GetAll(param);
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
                var param = new PostDto() { Limit = limit, Offset = 0, IsUpdate = true, SortBy = "updated_at" };
                string content = await _fileLogService.readFile("post-update");
                if (content != null && content != "")
                {
                    param.UpdatedDateFrom = DateTime.Parse(content);
                }
                var result = await _postCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            _postObRepository.ReplaceInto(new PostObModel()
                            {
                                PostId = item?.PostId,
                                Name = item?.Name,
                                Description = item?.Description,
                                UserId = item?.UserId,
                                CategoryId = item?.CategoryId,
                                CreatedAt = item?.CreatedAt,
                                UpdatedAt = item?.UpdatedAt
                            });

                        }
                        if (item?.UpdatedAt != null)
                        {
                            string dataContent = item?.UpdatedAt.ToString() ?? "";
                            if (dataContent != "")
                            {
                                await _fileLogService.writeFile("post-update", dataContent);
                            }
                        }
                    }
                    param.Offset += limit;
                    result = await _postCbRepository.GetAll(param);
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
                string content = await _fileLogService.readFile("post-delete");
                var param = new LogDto()
                {
                    ObjectName = "post",
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
                        _postObRepository.Delete(item?.ObjectId ?? 0);
                        if (item?.LogId != null && item?.LogId.ToString() != "")
                        {
                            string dataContent = item?.LogId.ToString() ?? "";
                            if (dataContent != "")
                            {
                                await _fileLogService.writeFile("post-delete", dataContent);
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
            //await SyncUpdate();
            //await SyncDelete();
        }
    }
}
