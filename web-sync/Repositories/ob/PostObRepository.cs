﻿using Dapper;
using Npgsql;
using web_sync.Dtos;
using web_sync.Models.ob;

namespace web_sync.Repositories.ob
{
    public interface IPostObRepository
    {
        Task<IEnumerable<PostObModel?>?> GetAll(PostDto param);
        Task<PostObModel?> GetById(int id);
        void Insert(PostObModel post);
        void ReplaceInto(PostObModel post);
        void BulkInsert(List<PostObModel> post);
        void Update(int id, PostObModel post);
        void Delete(int id);
    }

    public class PostObRepository : IPostObRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_posts";

        public PostObRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PostObModel?>?> GetAll(PostDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if (param.PostId != null)
            {
                where += " AND post_id = @PostId";
            }

            if (param.FromPostId != null)
            {
                where += " AND post_id > @FromPostId";
            }

            if (param.PostIds != null)
            {
                where += " AND post_id = ANY(@PostIds)";
            }

            if (param.UserId != null)
            {
                where += " AND user_id = @UserId";
            }

            if (param.CategoryId != null)
            {
                where += " AND category_id = @CategoryId";
            }

            if (param.CreatedDateTo != null)
            {
                where += " AND created_at <= @CreatedDateTo";
            }

            if (param.CreatedDateFrom != null)
            {
                where += " AND created_at >= @CreatedDateFrom";
            }

            if (param.UpdatedDateFrom != null)
            {
                where += " AND updated_at > @UpdatedDateFrom";
            }

            if (param.IsUpdate == true)
            {
                where += " AND created_at != updated_at";
            }

            if (param.Offset != null)
            {
                limit += " OFFSET " + param.Offset.ToString();
            }

            if (param.Limit != null)
            {
                limit += " LIMIT " + param.Limit.ToString();
            }

            if (param.SortBy != null)
            {
                string sortOrder = param.SortOrder != "desc" ? " asc" : " desc";
                string[] sortBy = { "post_id", "name", "user_id", "created_at" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy : sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "post_id", "user_id", "category_id", "name", "description", "created_at" };
                List<string> customField = new();
                foreach (string field in param.Fields)
                {
                    if (fieldAllow.Contains(field))
                    {
                        customField.Add(field);
                    }
                }
                if (customField.Count > 0)
                {
                    fields = string.Join(", ", customField);
                }
            }

            string sql = "SELECT " + fields + " FROM " + table;
            sql += where + sort + limit;

            var res = await _connection.QueryAsync<dynamic>(sql, param);
            if (res == null)
            {
                return null;
            }
            var data = res.Select(p => new PostObModel
            {
                PostId = p.post_id ?? null,
                Name = p.name ?? null,
                Description = p.description ?? null,
                UserId = p.user_id ?? null,
                CategoryId = p.category_id ?? null,
                CreatedAt = p.created_at ?? null,
                UpdatedAt = p.updated_at ?? null
            });
            return data;
        }

        public async Task<PostObModel?> GetById(int id)
        {
            if (id > 0)
            {
                string sql = "SELECT * FROM " + table + " WHERE post_id = @Id";
                var res = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
                if (res == null)
                {
                    return null;
                }
                var data = new PostObModel()
                {
                    PostId = res.post_id ?? null,
                    Name = res.name ?? null,
                    Description = res.description ?? null,
                    UserId = res.user_id ?? null,
                    CategoryId = res.category_id ?? null,
                    CreatedAt = res.created_at ?? null,
                    UpdatedAt = res.updated_at ?? null
                };
                return data;
            }
            return null;
        }

        public void Insert(PostObModel post)
        {
            List<string> column = new();
            List<string> columnData = new();
            if (post.PostId != null)
            {
                column.Add("post_id");
                columnData.Add("@PostId");
            }
            if (post.Name != null)
            {
                column.Add("name");
                columnData.Add("@Name");
            }
            if (post.Description != null)
            {
                column.Add("description");
                columnData.Add("@Description");
            }
            if (post.CategoryId != null)
            {
                column.Add("category_id");
                columnData.Add("@CategoryId");
            }
            if (post.UserId != null)
            {
                column.Add("user_id");
                columnData.Add("@UserId");
            }
            if (post.CreatedAt != null)
            {
                column.Add("created_at");
                columnData.Add("@CreatedAt");
            }
            if (post.UpdatedAt != null)
            {
                column.Add("updated_at");
                column.Add("@UpdatedAt");
            }
            string query = "INSERT INTO " + table + "(" + string.Join(", ", column) + ") VALUES (" + string.Join(", ", columnData) + ")";
            _connection.Execute(query, post);
        }

        public void BulkInsert(List<PostObModel> posts)
        {
            if (posts.Count > 0)
            {
                List<string> column = new();
                if (posts[0].PostId != null)
                {
                    column.Add("post_id");
                }
                if (posts[0].Name != null)
                {
                    column.Add("name");
                }
                if (posts[0].Description != null)
                {
                    column.Add("description");
                }
                if (posts[0].CategoryId != null)
                {
                    column.Add("category_id");
                }
                if (posts[0].UserId != null)
                {
                    column.Add("user_id");
                }
                if (posts[0].CreatedAt != null)
                {
                    column.Add("created_at");
                }
                if (posts[0].UpdatedAt != null)
                {
                    column.Add("updated_at");
                }
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column) + ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in posts)
                    {
                        writer.StartRow();
                        if (item.PostId != null)
                        {
                            writer.Write(item.PostId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.Name != null)
                        {
                            writer.Write(item.Name, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.Description != null)
                        {
                            writer.Write(item.Description, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.UserId != null)
                        {
                            writer.Write(item.Name, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.CategoryId != null)
                        {
                            writer.Write(item.Name, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.CreatedAt != null)
                        {
                            writer.Write(item.Name, NpgsqlTypes.NpgsqlDbType.Timestamp);
                        }
                        if (item.UpdatedAt != null)
                        {
                            writer.Write(item.Name, NpgsqlTypes.NpgsqlDbType.Timestamp);
                        }
                    }

                    writer.Complete();
                }

            }
        }

        public void ReplaceInto(PostObModel Post)
        {
            string query = "INSERT INTO " + table + "(";
            List<string> column = new();
            List<string> dataSet = new();
            List<string> dataUpdate = new();
            if (Post.Name != null)
            {
                column.Add("name");
                dataSet.Add("@Name");
                dataUpdate.Add("name = @Name");
            }
            if (Post.Description != null)
            {
                column.Add("description");
                dataSet.Add("@Description");
                dataUpdate.Add("description = @Description");
            }
            if (Post.CategoryId != null)
            {
                column.Add("category_id");
                dataSet.Add("@CategoryId");
                dataUpdate.Add("category_id = @CategoryId");
            }
            if (Post.PostId != null)
            {
                column.Add("post_id");
                dataSet.Add("@PostId");
            }
            if (Post.UserId != null)
            {
                column.Add("user_id");
                dataSet.Add("@UserId");
                dataUpdate.Add("user_id = @UserId");
            }
            if (Post.CreatedAt != null)
            {
                column.Add("created_at");
                dataSet.Add("@CreatedAt");
                dataUpdate.Add("created_at = @CreatedAt");
            }
            if (Post.UpdatedAt != null)
            {
                column.Add("updated_at");
                dataSet.Add("@UpdatedAt");
                dataUpdate.Add("updated_at = @UpdatedAt");
            }

            query += string.Join(", ", column) + ") VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (post_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Post);
        }

        public void Update(int id, PostObModel post)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new();
            post.PostId = id;
            if (post.Name != null)
            {
                dataSet.Add("name = @Name");
            }
            if (post.Description != null)
            {
                dataSet.Add("description = @Description");
            }
            if (post.CategoryId != null)
            {
                dataSet.Add("category_id = @CategoryId");
            }
            if (post.UserId != null)
            {
                dataSet.Add("user_id = @UserId");
            }
            if (post.CreatedAt != null)
            {
                dataSet.Add("created_at = @CreatedAt");
            }
            if (post.UpdatedAt != null)
            {
                dataSet.Add("updated_at = @UpdatedAt");
            }
            query += string.Join(", ", dataSet) + " WHERE post_id = @PostId";
            _connection.Execute(query, post);
        }

        public void Delete(int id)
        {
            if (id > 0)
            {
                string query = "DELETE FROM " + table + " WHERE post_id = @PostId";
                _connection.Execute(query, new { PostId = id });
            }

        }
    }
}
