﻿using web_sync.Repositories.ob;
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
        public PostService(PostObRepository PostObRepository,
            PostCbRepository PostCbRepository,
            FileLogService fileLogService,
            LogCbRepository logCbRepository
            )
        {
            _postObRepository = PostObRepository;
            _postCbRepository = PostCbRepository;
            _fileLogService = fileLogService;
            _logCbRepository = logCbRepository;
        }

        public async Task<bool> syncInsert()
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
                        await _fileLogService.writeFile("post-insert", item?.PostId.ToString() ?? "");
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

        public async Task<bool> syncInsertWithCondition(InsertDto insertParam)
        {
            try
            {
                int limit = 1000;
                var param = new PostDto() { Limit = limit, Offset = 0, PostIds = insertParam.id };
                var result = await _postCbRepository.GetAll(param);
                while (result != null && result.Any())
                {
                    foreach (var item in result)
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
        public async Task<bool> syncUpdateOrDelete()
        {
            try
            {
                int limit = 1000;
                string content = await _fileLogService.readFile("post-query-log");
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
                        if (item?.ObjectType == "update")
                        {
                            var PostData = await _postCbRepository.GetById(item.ObjectId ?? 0);
                            if (PostData != null)
                            {
                                _postObRepository.ReplaceInto(new PostObModel()
                                {
                                    PostId = PostData?.PostId,
                                    Name = PostData?.Name,
                                    Description = PostData?.Description,
                                    UserId = PostData?.UserId,
                                    CategoryId = PostData?.CategoryId,
                                    CreatedAt = PostData?.CreatedAt,
                                    UpdatedAt = PostData?.UpdatedAt
                                });

                            }

                        }
                        else
                        {
                            _postObRepository.Delete(item?.ObjectId ?? 0);
                        }
                        await _fileLogService.writeFile("post-query-log", item?.LogId?.ToString() ?? "");
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
