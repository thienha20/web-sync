using Dapper;
using Npgsql;
using web_sync.Dtos;
using web_sync.Models.ob;

namespace web_sync.Repositories.ob
{
    public interface IRegionObRepository
    {
        Task<IEnumerable<RegionObModel?>?> GetAll(RegionDto param);
        Task<RegionObModel?> GetById(int id);
        void Insert(RegionObModel Region);
        void ReplaceInto(RegionObModel Region);
        void BulkInsert(List<RegionObModel> Region);
        void Update(int id, RegionObModel Region);
        void Delete(int id);
    }

    public class RegionObRepository : IRegionObRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_regions";

        public RegionObRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<RegionObModel?>?> GetAll(RegionDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if (param.RegionId != null)
            {
                where += " AND region_id = @RegionId";
            }

            if (param.RegionIds != null)
            {
                where += " AND region_id = ANY(@RegionIds)";
            }

            if (param.RegionName != null)
            {
                param.RegionName = "%" + param.RegionName + "%";
                where += " AND region_name like @RegionName";
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
                string sortOrder = param.SortOrder != "asc" ? " desc" : " asc";
                string[] sortBy = { "region_id", "region_name" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy : sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "region_id", "region_name" };
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
            var data = res.Select(p => new RegionObModel
            {
                RegionId = p.region_id ?? null,
                RegionName = p.region_name ?? null,
            });
            return data;
        }

        public async Task<RegionObModel?> GetById(int id)
        {
            if (id > 0)
            {
                string sql = "SELECT * FROM " + table + " WHERE region_id = @Id";
                var res = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
                if (res == null)
                {
                    return null;
                }
                var data = new RegionObModel()
                {
                    RegionId = res.region_id ?? null,
                    RegionName = res.region_name ?? null,
                };
                return data;
            }
            return null;
        }

        public void Insert(RegionObModel Region)
        {
            List<string> column = new();
            List<string> columnData = new();
            if (Region.RegionId != null)
            {
                column.Add("region_id");
                columnData.Add("@RegionId");
            }
            if (Region.RegionName != null)
            {
                column.Add("region_name");
                columnData.Add("@RegionName");
            }

            string query = "INSERT INTO " + table + "(" + string.Join(", ", column) + ") VALUES (" + string.Join(", ", columnData) + ")";
            _connection.Execute(query, Region);
        }

        public void BulkInsert(List<RegionObModel> Regions)
        {
            if (Regions.Count > 0)
            {
                List<string> column = new();
                if (Regions[0].RegionId != null)
                {
                    column.Add("region_id");
                }
                if (Regions[0].RegionName != null)
                {
                    column.Add("region_name");
                }
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column) + ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in Regions)
                    {
                        writer.StartRow();
                        if (item.RegionId != null)
                        {
                            writer.Write(item.RegionId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.RegionName != null)
                        {
                            writer.Write(item.RegionName, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                    }

                    writer.Complete();
                }
            }
        }

        public void ReplaceInto(RegionObModel Region)
        {
            string query = "INSERT INTO " + table + "(";
            List<string> column = new();
            List<string> dataSet = new();
            List<string> dataUpdate = new();
            if (Region.RegionName != null)
            {
                column.Add("region_name");
                dataSet.Add("@RegionName");
                dataUpdate.Add("region_name = @RegionName");
            }

            if (Region.RegionId != null)
            {
                column.Add("region_id");
                dataSet.Add("@RegionId");
            }

            query += string.Join(", ", column) + ") VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (region_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Region);
        }

        public void Update(int id, RegionObModel Region)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new();
            Region.RegionId = id;

            if (Region.RegionName != null)
            {
                dataSet.Add("region_name = @RegionName");
            }

            query += string.Join(", ", dataSet) + " WHERE region_id = @RegionId";
            _connection.Execute(query, Region);
        }

        public void Delete(int id)
        {
            if (id > 0)
            {
                string query = "DELETE FROM " + table + " WHERE region_id = @RegionId";
                _connection.Execute(query, new { RegionId = id });

            }

        }
    }
}
