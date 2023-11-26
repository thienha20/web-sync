using Dapper;
using Npgsql;
using sync_data.Dtos;
using sync_data.Models.ob;

namespace sync_data.Repositories.ob
{
    public interface IRegionObRepository
    {
        IEnumerable<RegionObModel> GetAll(RegionDto param);
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

        public IEnumerable<RegionObModel> GetAll(RegionDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.RegionId != null)
            {
                where += " AND region_id = @RegionId";
            }

            if(param.RegionName != null)
            {
                param.RegionName = "%" + param.RegionName + "%";
                where += " AND region_name like @RegionName";
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
                string[] sortBy = { "region_id", "region_name" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "region_id", "region_name" };
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

            string sql = "SELECT " + fields + " FROM regions";
            sql += where + sort + limit;

            return _connection.Query<RegionObModel>(sql, param);
        }

        public void Insert(RegionObModel Region)
        {
            List<string> column = new List<string>();
            List<string> columnData = new List<string>();
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
                List<string> column = new List<string>();
                if (Regions[0].RegionId != null)
                {
                    column.Add("region_id");
                }
                if (Regions[0].RegionName != null)
                {
                    column.Add("region_name");
                }
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column)+ ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in Regions)
                    {
                        writer.StartRow();
                        if(item.RegionId != null)
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
            string query = "INSERT INTO " + table;
            List<string> column = new List<string>();
            List<string> dataSet = new List<string>();
            List<string> dataUpdate = new List<string>();
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

            query += string.Join(", ", column) + " VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (region_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Region);
        }

        public void Update(int id, RegionObModel Region)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new List<string>();
            if(Region.RegionName != null)
            {
                dataSet.Add("region_name = @RegionName");
            }
            
            if (Region.RegionId != null)
            {
                dataSet.Add("region_id = @RegionId");
            }
            
            query += string.Join(", ", dataSet) + " WHERE region_id = @RegionId";
            _connection.Execute(query, Region);
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM " + table + " WHERE region_id = @RegionId";
            _connection.Execute(query, new { RegionId = id } );
        }
    }
}
