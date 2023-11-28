using Dapper;
using Npgsql;
using web_sync.Dtos;
using web_sync.Models.cb;

namespace web_sync.Repositories.cb
{
    public interface IUserCbRepository
    {
        Task<IEnumerable<UserCbModel?>?> GetAll(UserDto param);
        Task<UserCbModel?> GetById(long id);
        void Insert(UserCbModel User);
        void ReplaceInto(UserCbModel User);
        void BulkInsert(List<UserCbModel> User);
        void Update(long id, UserCbModel User);
        void Delete(long id);
    }

    public class UserCbRepository : IUserCbRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_users";
        public UserCbRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<UserCbModel?>?> GetAll(UserDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.UserId != null)
            {
                where += " AND user_id = @UserId";
            }

            if (param.FromUserId != null)
            {
                where += " AND user_id > @FromUserId";
            }

            if (param.UserIds != null) 
            { 
                where += " AND user_id = ANY(@UserIds)";
            }

            if (param.Email != null)
            {
                param.Email = "%" + param.Email + "%";
                where += " AND email like @Email";
            }

            if (param.CountryId != null)
            {
                where += " AND country_id = @CountryId";
            }

            if(param.CreatedDateTo != null)
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
                string[] sortBy = { "user_id", "created_at", "country_id", "email", "full_name", "username" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "user_id", "full_name", "email", "username", "country_id", "created_at" };
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
            try
            {
                var res = await _connection.QueryAsync<dynamic>(sql, param);
                if (res == null)
                {
                    return null;
                }
                var data = res.Select(p => new UserCbModel
                {
                    UserId = p.user_id ?? null,
                    UserName = p.user_name ?? null,
                    FullName = p.full_name ?? null,
                    Email = p.email ?? null,
                    CountryId = p.country_id ?? null,
                    CreatedAt = p.created_at ?? null,
                    UpdatedAt = p.updated_at ?? null,
                });
                return data;
            } catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<UserCbModel?> GetById(long id)
        {
            if (id > 0)
            {
                string sql = "SELECT * FROM " + table + " WHERE user_id = @Id";
                var res = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
                if (res == null)
                {
                    return null;
                }
                var data = new UserCbModel()
                {
                    UserId = res.user_id ?? null,
                    UserName = res.user_name ?? null,
                    FullName = res.full_name ?? null,
                    Email = res.email ?? null,
                    CountryId = res.country_id ?? null,
                    CreatedAt = res.created_at ?? null,
                    UpdatedAt = res.updated_at ?? null,
                };
                return data;
            }
            return null;
        }

        public void Insert(UserCbModel User)
        {
            List<string> column = new();
            List<string> columnData = new();
            if (User.FullName != null)
            {
                column.Add("full_name");
                columnData.Add("@FullName");
            }
            if (User.UserName != null)
            {
                column.Add("username");
                columnData.Add("@UserName");
            }
            if (User.Email != null)
            {
                column.Add("email");
                columnData.Add("@Email");
            }
            if (User.CountryId != null)
            {
                column.Add("country_id");
                columnData.Add("@CountryId");
            }
            if (User.UserId != null)
            {
                column.Add("user_id");
                columnData.Add("@UserId");
            }
            if (User.CreatedAt != null)
            {
                column.Add("created_at");
                columnData.Add("@CreatedAt");
            }
            if (User.UpdatedAt != null)
            {
                column.Add("updated_at");
                column.Add("@UpdatedAt");
            }
            string query = "INSERT INTO " + table + "(" + string.Join(", ", column) + ") VALUES (" + string.Join(", ", columnData) + ")";
            _connection.Execute(query, User);
        }

        public void BulkInsert(List<UserCbModel> Users)
        {
            if (Users.Count > 0)
            {
                List<string> column = new();
                if (Users[0].FullName != null)
                {
                    column.Add("full_name");
                }
                if (Users[0].UserName != null)
                {
                    column.Add("username");
                }
                if (Users[0].Email != null)
                {
                    column.Add("email");
                }
                if (Users[0].CountryId != null)
                {
                    column.Add("country_id");
                }
                if (Users[0].UserId != null)
                {
                    column.Add("user_id");
                }
                if (Users[0].CreatedAt != null)
                {
                    column.Add("created_at");
                }
                if (Users[0].UpdatedAt != null)
                {
                    column.Add("updated_at");
                }
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column)+ ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in Users)
                    {
                        writer.StartRow();
                        if(item.FullName != null)
                        {
                            writer.Write(item.FullName, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.UserName != null)
                        {
                            writer.Write(item.UserName, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.Email != null)
                        {
                            writer.Write(item.Email, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.UserId != null)
                        {
                            writer.Write(item.UserId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.CountryId != null)
                        {
                            writer.Write(item.CountryId, NpgsqlTypes.NpgsqlDbType.Integer);
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

                string query = "INSERT INTO Users(name, description, user_id, category_id) VALUES (@Name, @Description, @UserId, @CategoryId)";
                _connection.Execute(query, Users);
            }
        }

        public void ReplaceInto(UserCbModel User)
        {
            string query = "INSERT INTO " + table + "(";
            List<string> column = new();
            List<string> dataSet = new();
            List<string> dataUpdate = new();
            if (User.FullName != null)
            {
                column.Add("full_name");
                dataSet.Add("@FullName");
                dataUpdate.Add("full_name = @FullName");
            }
            if (User.Email != null)
            {
                column.Add("email");
                dataSet.Add("@Email");
                dataUpdate.Add("email = @Email");
            }
            if (User.UserName != null)
            {
                column.Add("username");
                dataSet.Add("@UserName");
                dataUpdate.Add("username = @UserName");
            }
            if (User.CountryId != null)
            {
                column.Add("country_id");
                dataSet.Add("@CountryId");
                dataUpdate.Add("country_id = @CountryId");
            }
            if (User.UserId != null)
            {
                column.Add("user_id");
                dataSet.Add("@UserId");
            }
            if (User.CreatedAt != null)
            {
                column.Add("created_at");
                dataSet.Add("@CreatedAt");
                dataUpdate.Add("created_at = @CreatedAt");
            }
            if (User.UpdatedAt != null)
            {
                column.Add("updated_at");
                dataSet.Add("@UpdatedAt");
                dataUpdate.Add("updated_at = @UpdatedAt");
            }

            query += string.Join(", ", column) + ") VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (user_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, User);
        }

        public void Update(long id, UserCbModel User)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new();
            User.UserId = id;
            if (User.FullName != null)
            {
                dataSet.Add("full_name = @FullName");
            }
            if (User.UserName != null)
            {
                dataSet.Add("username = @UserName");
            }
            if (User.Email != null)
            {
                dataSet.Add("email = @Email");
            }
            if (User.CountryId != null)
            {
                dataSet.Add("country_id = @CountryId");
            }
            
            if (User.CreatedAt != null)
            {
                dataSet.Add("created_at = @CreatedAt");
            }
            if (User.UpdatedAt != null)
            {
                dataSet.Add("updated_at = @UpdatedAt");
            }
            query += string.Join(", ", dataSet) + " WHERE user_id = @UserId";
            _connection.Execute(query, User);
        }

        public void Delete(long id)
        {
            if(id > 0)
            {
                string query = "DELETE FROM " + table + " WHERE user_id = @UserId";
                _connection.Execute(query, new { UserId = id });
            }
            
        }
    }
}
