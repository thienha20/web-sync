﻿using Dapper;
using Npgsql;
using web_sync.Dtos;
using web_sync.Models.cb;

namespace web_sync.Repositories.cb
{
    public interface ICategoryCbRepository
    {
        Task<IEnumerable<CategoryCbModel?>?> GetAll(CategoryDto param);
        Task<CategoryCbModel?> GetById(int id);
        void Insert(CategoryCbModel Category);
        void ReplaceInto(CategoryCbModel Category);
        void BulkInsert(List<CategoryCbModel> Category);
        void Update(int id, CategoryCbModel Category);
        void Delete(int id);
    }

    public class CategoryCbRepository : ICategoryCbRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_categories";

        public CategoryCbRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<CategoryCbModel?>?> GetAll(CategoryDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.CategoryId != null)
            {
                where += " AND category_id = @CategoryId";
            }

            if (param.FromCategoryId != null)
            {
                where += " AND category_id > @FromCategoryId";
            }

            if (param.CategoryIds != null)
            {
                where += " AND category_id = ANY(@CategoryIds)";
            }

            if (param.ParentId != null)
            {
                where += " AND parent_id = @ParentId";
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
                string sortOrder = param.SortOrder != "desc" ? " asc": " desc";
                string[] sortBy = { "category_id", "name", "created_at", "updated_at" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "parent_id", "category_id", "name", "description", "created_at", "path" };
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
            var data = res.Select(p => new CategoryCbModel
            {
                CategoryId = p.category_id ?? null,
                Name = p.name ?? null,
                Description = p.description ?? null,
                ParentId = p.parent_id ?? null,
                Path = p.path ?? null,
                CreatedAt = p.created_at ?? null,
                UpdatedAt = p.updated_at ?? null
            });
            return data;
        }

        public async Task<CategoryCbModel?> GetById(int id)
        {
            if (id > 0)
            {
                string sql = "SELECT * FROM " + table + " WHERE category_id = @Id";
                var res = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
                if (res == null)
                {
                    return null;
                }
                var data = new CategoryCbModel()
                {
                    CategoryId = res.category_id ?? null,
                    Name = res.name ?? null,
                    Description = res.description ?? null,
                    ParentId = res.parent_id ?? null,
                    Path = res.path ?? null,
                    CreatedAt = res.created_at ?? null,
                    UpdatedAt = res.updated_at ?? null
                };
                return data;
            }
            return null;
        }

        public void Insert(CategoryCbModel Category)
        {
            List<string> column = new();
            List<string> columnData = new();
            if (Category.Path != null)
            {
                column.Add("path");
                columnData.Add("@Path");
            }
            if (Category.Name != null)
            {
                column.Add("name");
                columnData.Add("@Name");
            }
            if (Category.Description != null)
            {
                column.Add("description");
                columnData.Add("@Description");
            }
            if (Category.CategoryId != null)
            {
                column.Add("category_id");
                columnData.Add("@CategoryId");
            }
            if (Category.ParentId != null)
            {
                column.Add("parent_id");
                columnData.Add("@ParentId");
            }
            if (Category.CreatedAt != null)
            {
                column.Add("created_at");
                columnData.Add("@CreatedAt");
            }
            if (Category.UpdatedAt != null)
            {
                column.Add("updated_at");
                column.Add("@UpdatedAt");
            }
            string query = "INSERT INTO " + table + "(" + string.Join(", ", column) + ") VALUES (" + string.Join(", ", columnData) + ")";
            _connection.Execute(query, Category);
        }

        public void BulkInsert(List<CategoryCbModel> Categories)
        {
            if (Categories.Count > 0)
            {
                List<string> column = new();
                if (Categories[0].Name != null)
                {
                    column.Add("name");
                }
                if (Categories[0].Path != null)
                {
                    column.Add("path");
                }
                if (Categories[0].Description != null)
                {
                    column.Add("description");
                }
                if (Categories[0].CategoryId != null)
                {
                    column.Add("category_id");
                }
                if (Categories[0].ParentId != null)
                {
                    column.Add("parent_id");
                }
                if (Categories[0].CreatedAt != null)
                {
                    column.Add("created_at");
                }
                if (Categories[0].UpdatedAt != null)
                {
                    column.Add("updated_at");
                }
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column)+ ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in Categories)
                    {
                        writer.StartRow();
                        if(item.Name != null)
                        {
                            writer.Write(item.Name, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.Description != null)
                        {
                            writer.Write(item.Description, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.ParentId != null)
                        {
                            writer.Write(item.ParentId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.Path != null)
                        {
                            writer.Write(item.Path, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.CreatedAt != null)
                        {
                            writer.Write(item.CreatedAt, NpgsqlTypes.NpgsqlDbType.Timestamp);
                        }
                        if (item.UpdatedAt != null)
                        {
                            writer.Write(item.UpdatedAt, NpgsqlTypes.NpgsqlDbType.Timestamp);
                        }
                    }

                    writer.Complete();
                }
            }
        }

        public void ReplaceInto(CategoryCbModel Category)
        {
            string query = "INSERT INTO " + table + "(";
            List<string> column = new();
            List<string> dataSet = new();
            List<string> dataUpdate = new();
            if (Category.Path != null)
            {
                column.Add("path");
                dataSet.Add("@Path");
                dataUpdate.Add("path = @Path");
            }
            if (Category.Name != null)
            {
                column.Add("name");
                dataSet.Add("@Name");
                dataUpdate.Add("name = @Name");
            }
            if (Category.Description != null)
            {
                column.Add("description");
                dataSet.Add("@Description");
                dataUpdate.Add("description = @Description");
            }
            if (Category.CategoryId != null)
            {
                column.Add("category_id");
                dataSet.Add("@CategoryId");
            }
            if (Category.ParentId != null)
            {
                column.Add("parent_id");
                dataSet.Add("@ParentId");
                dataUpdate.Add("parent_id = @ParentId");
            }
            if (Category.CreatedAt != null)
            {
                column.Add("created_at");
                dataSet.Add("@CreatedAt");
                dataUpdate.Add("created_at = @CreatedAt");
            }
            if (Category.UpdatedAt != null)
            {
                column.Add("updated_at");
                dataSet.Add("@UpdatedAt");
                dataUpdate.Add("updated_at = @UpdatedAt");
            }
            query += string.Join(", ", column) + ") VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (category_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Category);
        }

        public void Update(int id, CategoryCbModel Category)
        {
            string query = "UPDATE " + table + " SET ";
            Category.CategoryId = id;
            List<string> dataSet = new();
            if (Category.Path != null)
            {
                dataSet.Add("path = @Path");
            }
            if (Category.Name != null)
            {
                dataSet.Add("name = @Name");
            }
            if (Category.Description != null)
            {
                dataSet.Add("description = @Description");
            }
            if (Category.CategoryId != null)
            {
                dataSet.Add("category_id = @CategoryId");
            }
            if (Category.ParentId != null)
            {
                dataSet.Add("parent_id = @ParentId");
            }
            if (Category.CreatedAt != null)
            {
                dataSet.Add("created_at = @CreatedAt");
            }
            if (Category.UpdatedAt != null)
            {
                dataSet.Add("updated_at = @UpdatedAt");
            }
            query += string.Join(", ", dataSet) + " WHERE category_id = @CategoryId";
            _connection.Execute(query, Category);
        }
        
        public void Delete(int id)
        {
            if(id > 0)
            {
                string query = "DELETE FROM " + table + " WHERE category_id = @CategoryId";
                _connection.Execute(query, new { CategoryId = id });
            }
        }
    }
}
