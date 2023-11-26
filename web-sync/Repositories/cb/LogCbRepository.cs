using Dapper;
using Npgsql;
using sync_data.Dtos;
using sync_data.Models.cb;

namespace sync_data.Repositories.cb
{
    public interface ILogCbRepository
    {
        IEnumerable<LogCbModel> GetAll(LogDto param);
        void Insert(LogCbModel Log);
        void ReplaceInto(LogCbModel Log);
        void BulkInsert(List<LogCbModel> Log);
        void Update(int id, LogCbModel Log);
        void Delete(int id);
    }

    public class LogCbRepository : ILogCbRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_queries_logs";
        public LogCbRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<LogCbModel> GetAll(LogDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.LogId != null)
            {
                where += " AND log_id = @LogId";
            }

            if(param.ObjectName != null)
            {
                where += " AND object_name = @ObjectName";
            }

            if(param.ObjectType != null)
            {
                where += " AND object_type = @ObjectType";
            }

            if (param.ObjectId != null)
            {
                where += " AND object_id = @ObjectId";
            }

            if (param.CreatedDateFrom != null)
            {
                where += " AND timestamps >= @CreatedDateFrom";
            }

            if (param.CreatedDateTo != null)
            {
                where += " AND timestamps <= @CreatedDateFrom";
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
                string[] sortBy = { "log_id", "object_type", "object_name", "timestamps" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "log_id", "created_at", "object_type", "object_name", "object_id", "timestamps" };
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

            string sql = "SELECT " + fields + " FROMdb_queries_logs";
            sql += where + sort + limit;

            return _connection.Query<LogCbModel>(sql, param);
        }

        public void Insert(LogCbModel Log)
        {
            List<string> column = new List<string>();
            List<string> columnData = new List<string>();
            if (Log.LogId != null)
            {
                column.Add("log_id");
                columnData.Add("@LogId");
            }
            if (Log.ObjectName != null)
            {
                column.Add("object_name");
                columnData.Add("@ObjectName");
            }
            if (Log.ObjectType != null)
            {
                column.Add("object_type");
                columnData.Add("@ObjectType");
            }
            if (Log.ObjectId != null)
            {
                column.Add("object_id");
                columnData.Add("@ObjectId");
            }
            if (Log.Timestamps != null)
            {
                column.Add("timestamps");
                columnData.Add("@Timestamps");
            }
            string query = "INSERT INTO " + table + "(" + string.Join(", ", column) + ") VALUES (" + string.Join(", ", columnData) + ")";
            _connection.Execute(query, Log);
        }

        public void BulkInsert(List<LogCbModel> Logs)
        {
            if (Logs.Count > 0)
            {
                List<string> column = new List<string>();
                if (Logs[0].LogId != null)
                {
                    column.Add("log_id");
                }
                if (Logs[0].ObjectName != null)
                {
                    column.Add("object_name");
                }
                if (Logs[0].ObjectType != null)
                {
                    column.Add("object_type");
                }
                if (Logs[0].ObjectId != null)
                {
                    column.Add("object_id");
                }
                if (Logs[0].Timestamps != null)
                {
                    column.Add("timestamps");
                }
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column)+ ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in Logs)
                    {
                        writer.StartRow();
                        if(item.LogId != null)
                        {
                            writer.Write(item.LogId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.ObjectName != null)
                        {
                            writer.Write(item.ObjectName, NpgsqlTypes.NpgsqlDbType.Text);
                        }

                        if (item.ObjectType != null)
                        {
                            writer.Write(item.ObjectType, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.ObjectId != null)
                        {
                            writer.Write(item.ObjectId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.Timestamps != null)
                        {
                            writer.Write(item.Timestamps, NpgsqlTypes.NpgsqlDbType.Timestamp);
                        }
                        
                    }

                    writer.Complete();
                }
            }
        }

        public void ReplaceInto(LogCbModel Log)
        {
            string query = "INSERT INTO " + table;
            List<string> column = new List<string>();
            List<string> dataSet = new List<string>();
            List<string> dataUpdate = new List<string>();
            if (Log.ObjectName != null)
            {
                column.Add("object_name");
                dataSet.Add("@ObjectName");
                dataUpdate.Add("object_name = @ObjectName");
            }
            if (Log.ObjectType != null)
            {
                column.Add("object_type");
                dataSet.Add("@ObjectType");
                dataUpdate.Add("object_type = @ObjectType");
            }
            if (Log.ObjectId != null)
            {
                column.Add("object_id");
                dataSet.Add("@ObjectId");
                dataUpdate.Add("object_id = @ObjectId");
            }
            if (Log.LogId != null)
            {
                column.Add("log_id");
                dataSet.Add("@LogId");
            }
            if (Log.Timestamps != null)
            {
                column.Add("timestamps");
                dataSet.Add("@Timestamps");
                dataUpdate.Add("timestamps = @Timestamps");
            }
            
            query += string.Join(", ", column) + " VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (log_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Log);
        }

        public void Update(int id, LogCbModel Log)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new List<string>();
            if(Log.LogId != null)
            {
                dataSet.Add("log_id = @LogId");
            }
            if (Log.ObjectName != null)
            {
                dataSet.Add("object_name = @ObjectName");
            }
            if (Log.ObjectType != null)
            {
                dataSet.Add("object_type = @ObjectType");
            }
            if (Log.ObjectId != null)
            {
                dataSet.Add("object_id = @ObjectId");
            }
            if (Log.Timestamps != null)
            {
                dataSet.Add("timestamps = @Timestamps");
            }
            query += string.Join(", ", dataSet) + " WHERE log_id = @LogId";
            _connection.Execute(query, Log);
            // Thực hiện logic để cập nhật entity trong cơ sở dữ liệu
        }

        public void Delete(int id)
        {
            // Thực hiện logic để xóa entity từ cơ sở dữ liệu
            string query = "DELETE FROM " + table + " WHERE log_id = @LogId";
            _connection.Execute(query, new { LogId = id } );
        }
    }
}
