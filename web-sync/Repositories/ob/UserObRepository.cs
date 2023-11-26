using Dapper;
using Npgsql;
using sync_data.Dtos;
using sync_data.Models.ob;

namespace sync_data.Repositories.ob
{
    public interface IUserObRepository
    {
        IEnumerable<UserObModel> GetAll(UserDto param);
        void Insert(UserObModel User);
        void ReplaceInto(UserObModel User);
        void BulkInsert(List<UserObModel> User);
        void Update(int id, UserObModel User);
        void Delete(int id);
    }

    public class UserObRepository : IUserObRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_users";
        public UserObRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<UserObModel> GetAll(UserDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.UserId != null)
            {
                where += " AND user_id = @UserId";
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
                string[] sortBy = { "user_id", "created_at", "country_id", "email", "full_name", "username" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "user_id", "full_name", "email", "username", "country_id", "created_at" };
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

            return _connection.Query<UserObModel>(sql, param);
        }

        public void Insert(UserObModel User)
        {
            List<string> column = new List<string>();
            List<string> columnData = new List<string>();
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

        public void BulkInsert(List<UserObModel> Users)
        {
            if (Users.Count > 0)
            {
                List<string> column = new List<string>();
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

        public void ReplaceInto(UserObModel User)
        {
            string query = "INSERT INTO " + table;
            List<string> column = new List<string>();
            List<string> dataSet = new List<string>();
            List<string> dataUpdate = new List<string>();
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

            query += string.Join(", ", column) + " VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (user_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, User);
        }

        public void Update(int id, UserObModel User)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new List<string>();
            if(User.FullName != null)
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
            if (User.UserId != null)
            {
                dataSet.Add("user_id = @UserId");
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
            // Thực hiện logic để cập nhật entity trong cơ sở dữ liệu
        }

        public void Delete(int id)
        {
            // Thực hiện logic để xóa entity từ cơ sở dữ liệu
            string query = "DELETE FROM " + table + " WHERE user_id = @UserId";
            _connection.Execute(query, new { UserId = id } );
        }
    }
}
