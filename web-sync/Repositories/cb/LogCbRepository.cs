﻿using Dapper;
using Npgsql;
using web_sync.Dtos;
using web_sync.Models.cb;

namespace web_sync.Repositories.cb
{
    public interface ILogCbRepository
    {
        Task<IEnumerable<LogCbModel?>?> GetAll(LogDto param);
        Task<LogCbModel?> GetById(long id);
        void Insert(LogCbModel Log);
        void ReplaceInto(LogCbModel Log);
        void BulkInsert(List<LogCbModel> Log);
        void Update(long id, LogCbModel Log);
        void Delete(long id);
    }

    public class LogCbRepository : ILogCbRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_queries_logs";
        public LogCbRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<LogCbModel?>?> GetAll(LogDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.LogId != null)
            {
                where += " AND log_id = @LogId";
            }

            if (param.FromLogId != null)
            {
                where += " AND log_id > @FromLogId";
            }

            if (param.ObjectName != null)
            {
                where += " AND object_name = @ObjectName";
            }

            if (param.ObjectNames != null)
            {
                where += " AND object_name = ANY(@ObjectNames)";
            }

            if (param.ObjectType != null)
            {
                where += " AND object_type = @ObjectType";
            }

            if (param.ObjectTypes != null)
            {
                where += " AND object_type = ANY(@ObjectTypes)";
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
                string sortOrder = param.SortOrder != "asc" ? " desc": " asc" ;
                string[] sortBy = { "log_id", "object_type", "object_name", "timestamps" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "log_id", "created_at", "object_type", "object_name", "object_id", "timestamps" };
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
                var data = res.Select(p => new LogCbModel
                {
                    LogId = p.log_id ?? null,
                    ObjectName = p.object_name ?? null,
                    ObjectType = p.object_type ?? null,
                    ObjectId = p.object_id ?? null,
                    Timestamps = p.timestamps ?? null,
                });
                return data;
            }
            catch
            {
                return null;
            }
            
        }

        public async Task<LogCbModel?> GetById(long id)
        {
            if (id > 0)
            {
                string sql = "SELECT * FROM " + table + " WHERE log_id = @Id";
                var res = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
                if (res == null)
                {
                    return null;
                }
                var data = new LogCbModel()
                {
                    LogId = res.log_id ?? null,
                    ObjectName = res.object_name ?? null,
                    ObjectType = res.object_type ?? null,
                    ObjectId = res.object_id ?? null,
                    Timestamps = res.timestamps ?? null,
                };
                return data;
            }
            return null;
        }

        public void Insert(LogCbModel Log)
        {
            List<string> column = new();
            List<string> columnData = new();
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
                List<string> column = new();
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
            string query = "INSERT INTO " + table + "(";
            List<string> column = new();
            List<string> dataSet = new();
            List<string> dataUpdate = new();
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
            
            query += string.Join(", ", column) + ") VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (log_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Log);
        }

        public void Update(long id, LogCbModel Log)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new();
            Log.LogId = id;
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
        }

        public void Delete(long id)
        {
            if (id > 0)
            {
                string query = "DELETE FROM " + table + " WHERE log_id = @LogId";
                _connection.Execute(query, new { LogId = id });
            }
        }
    }
}
