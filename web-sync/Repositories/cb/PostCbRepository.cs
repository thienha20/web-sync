using Dapper;
using Npgsql;
using sync_data.Dtos;
using sync_data.Models.cb;

namespace sync_data.Repositories.cb
{
    public interface IPostCbRepository
    {
        IEnumerable<PostCbModel> GetAll(PostDto param);
        void Insert(PostCbModel post);
        void ReplaceInto(PostCbModel post);
        void BulkInsert(List<PostCbModel> post);
        void Update(int id, PostCbModel post);
        void Delete(int id);
    }

    public class PostCbRepository : IPostCbRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_posts";

        public PostCbRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<PostCbModel> GetAll(PostDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.UserId != null)
            {
                where += " AND user_id = @UserId";
            }

            if(param.CategoryId != null)
            {
                where += " AND category_id = @CategoryId";
            }

            if(param.CreatedDateTo != null)
            {
                where += " AND created_at <= @CreatedDateTo";
            }

            if (param.CreatedDateFrom != null)
            {
                where += " AND created_at >= @CreatedDateFrom";
            }

            if (param.Limit != null)
            {
                if(param.Offset != null)
                {
                    limit += " limit " + param.Offset.ToString() + ", " + param.Limit.ToString();
                }
                else
                {
                    limit += " limit " + param.Limit.ToString();
                }     
            }

            if(param.SortBy != null)
            {
                string sortOrder = param.SortOrder != "asc" ? " desc": " asc" ;
                string[] sortBy = { "post_id", "name", "user_id", "created_at"  };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "post_id", "user_id", "category_id", "name", "description", "created_at" };
                List<string> customField = new List<string>();
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

            return _connection.Query<PostCbModel>(sql, param);
        }

        public void Insert(PostCbModel post)
        {
            List<string> column = new List<string>();
            List<string> columnData = new List<string>();
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

        public void BulkInsert(List<PostCbModel> posts)
        {
            if (posts.Count > 0)
            {
                List<string> column = new List<string>();
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
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column)+ ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in posts)
                    {
                        writer.StartRow();
                        if (item.PostId  != null)
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

        public void ReplaceInto(PostCbModel Post)
        {
            string query = "INSERT INTO " + table;
            List<string> column = new List<string>();
            List<string> dataSet = new List<string>();
            List<string> dataUpdate = new List<string>();
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

            query += string.Join(", ", column) + " VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (post_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Post);
        }

        public void Update(int id, PostCbModel post)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new List<string>();
            if (post.PostId != null)
            {
                dataSet.Add("post_id = @PostId");
            }
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
            string query = "DELETE FROM " + table + " WHERE post_id = @PostId";
            _connection.Execute(query, new { PostId = id } );
        }
    }
}
