using web_sync.Repositories.ob;
using web_sync.Repositories.cb;
using web_sync.Models.ob;
using web_sync.Dtos;
using web_sync.Dtos.SyncParam;
namespace web_sync.Services
{
    public class CategoryService
    {
        private readonly CategoryObRepository _categoryObRepository;
        private readonly CategoryCbRepository _categoryCbRepository;
        private readonly FileLogService _fileLogService;
        private readonly LogCbRepository _logCbRepository;
        public CategoryService(CategoryObRepository categoryObRepository, 
            CategoryCbRepository categoryCbRepository,
            FileLogService fileLogService,
            LogCbRepository logCbRepository)
        {
            _categoryObRepository = categoryObRepository;
            _categoryCbRepository = categoryCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
        }

        public async Task<bool> SyncInsert()
        {
            try
            {
                int limit = 1000;
                var param = new CategoryDto() { Limit = limit, Offset = 0 };
                string content = await _fileLogService.readFile("category-insert");
                if(content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out int fromCategoryId);
                    if (isValidInt)
                    {
                        param.FromCategoryId = fromCategoryId;
                    }
                }
                var result = await _categoryCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        _categoryObRepository.ReplaceInto(new CategoryObModel()
                        {
                            CategoryId = item?.CategoryId,
                            Name = item?.Name,
                            Description = item?.Description,
                            Path = item?.Path,
                            ParentId = item?.ParentId,
                            CreatedAt = item?.CreatedAt,
                            UpdatedAt = item?.UpdatedAt
                        });
                        if (item?.CategoryId != null)
                        {
                            string dataContent = item?.CategoryId.ToString() ?? "";
                            if (dataContent != "")
                            {
                                await _fileLogService.writeFile("category-insert", dataContent);
                            }
                        }
                    }
                    param.Offset += limit;
                    result = await _categoryCbRepository.GetAll(param);
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
                var param = new CategoryDto() { Limit = limit, Offset = 0, CategoryIds = insertParam.id };
                var result = await _categoryCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        _categoryObRepository.ReplaceInto(new CategoryObModel()
                        {
                            CategoryId = item?.CategoryId,
                            Name = item?.Name,
                            Description = item?.Description,
                            Path = item?.Path,
                            ParentId = item?.ParentId,
                            CreatedAt = item?.CreatedAt,
                            UpdatedAt = item?.UpdatedAt
                        });
                    }
                    param.Offset += limit;
                    result = await _categoryCbRepository.GetAll(param);
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
                var param = new CategoryDto() { Limit = limit, Offset = 0, IsUpdate=true, SortBy = "updated_at" };
                string content = await _fileLogService.readFile("category-update");
                if (content != null && content != "")
                {
                    param.UpdatedDateFrom = DateTime.Parse(content);
                }
                var result = await _categoryCbRepository.GetAll(param);

                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        _categoryObRepository.ReplaceInto(new CategoryObModel()
                        {
                            CategoryId = item?.CategoryId,
                            Name = item?.Name,
                            Description = item?.Description,
                            Path = item?.Path,
                            ParentId = item?.ParentId,
                            CreatedAt = item?.CreatedAt,
                            UpdatedAt = item?.UpdatedAt
                        });
                        if (item?.UpdatedAt != null)
                        {
                            string dataContent = item?.UpdatedAt.ToString() ?? "";
                            if (dataContent != "") { 
                                await _fileLogService.writeFile("category-update", dataContent);
                            }
                        }
                    }
                    param.Offset += limit;
                    result = await _categoryCbRepository.GetAll(param);
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
                string content = await _fileLogService.readFile("category-delete");
                var param = new LogDto()
                {
                    ObjectName = "category",
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
                        _categoryObRepository.Delete(item?.ObjectId ?? 0);
                        if (item?.LogId != null && item?.LogId.ToString() != "")
                        {
                            string dataContent = item?.LogId.ToString() ?? "";
                            if(dataContent != "")
                            {
                                await _fileLogService.writeFile("category-delete", dataContent);
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
