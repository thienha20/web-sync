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
            LogCbRepository logCbRepository
            )
        {
            _categoryObRepository = categoryObRepository;
            _categoryCbRepository = categoryCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
        }

        public async Task<bool> syncInsert()
        {
            try
            {
                int limit = 1000, fromCategoryId;
                var param = new CategoryDto() { Limit = limit, Offset = 0 };
                string content = await _fileLogService.readFile("category-insert");
                if(content != null && content != "")
                {
                    bool isValidInt = int.TryParse(content, out fromCategoryId);
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
                            CategoryId = item.CategoryId,
                            Name = item.Name
                        });
                        await _fileLogService.writeFile("category-insert", item.categoryId.ToString() ?? "");
                    }
                    param.Offset = param.Offset + limit;
                    result = await _categoryCbRepository.GetAll(param);
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
                var param = new CategoryDto() { Limit = limit, Offset = 0, CategoryIds = insertParam.id };
                var result = await _categoryCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        _categoryObRepository.ReplaceInto(new CategoryObModel()
                        {
                            CategoryId = item.categoryId,
                            Name = item.Name
                        });
                    }
                    param.Offset = param.Offset + limit;
                    result = await _categoryCbRepository.GetAll(param);
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
                string content = await _fileLogService.readFile("category-query-log");
                var param = new LogDto() {
                    ObjectName = "category",
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
                            var categoryData = await _categoryCbRepository.GetById(item.ObjectId ?? 0);
                            if(categoryData != null)
                            {
                                _categoryObRepository.ReplaceInto(new CategoryObModel()
                                {
                                    CategoryId = categoryData.categoryId,
                                    Name = categoryData.Name
                                });

                            }
                            
                        } else
                        {
                            _categoryObRepository.Delete(item.ObjectId ?? 0);
                        }
                        await _fileLogService.writeFile("category-query-log", item.LogId.ToString() ?? "");
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
