using Dapper;
using Npgsql;
using sync_data.Dtos;
using sync_data.Models.cb;

namespace sync_data.Repositories.cb
{
    public interface ICountryCbRepository
    {
        Task<IEnumerable<CountryCbModel>> GetAll(CountryDto param);
        void Insert(CountryCbModel Country);
        void ReplaceInto(CountryCbModel Country);
        void BulkInsert(List<CountryCbModel> Country);
        void Update(int id, CountryCbModel Country);
        void Delete(int id);
    }

    public class CountryCbRepository : ICountryCbRepository
    {
        private readonly NpgsqlConnection _connection;
        public string table { get; set; } = "db_countries";

        public CountryCbRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<CountryCbModel>> GetAll(CountryDto param)
        {
            string fields = "*";
            string where = " WHERE true ";
            string limit = "";
            string sort = "";

            if(param.CountryId != null)
            {
                where += " AND country_id = @CountryId";
            }

            if (param.CountryCode != null)
            {
                where += " AND country_code = @CountryCode";
            }

            if (param.CountryName != null)
            {
                param.CountryName = "%" + param.CountryName + "%";
                where += " AND country_name like @CountryName";
            }

            if (param.RegionId != null)
            {
                where += " AND region_id = @RegionId";
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
                string[] sortBy = { "country_id", "region_id", "country_code", "country_name" };
                sort += " ORDER BY " + (sortBy.Contains(param.SortBy) ? param.SortBy: sortBy[0]) + sortOrder;
            }

            if (param.Fields != null)
            {
                string[] fieldAllow = { "country_id", "region_id", "country_code", "country_name"};
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
            var res = await _connection.QueryAsync<dynamic>(sql, param);
            var data = res.Select(p => new CountryCbModel
            {
                RegionId = p.region_id ?? null,
                CountryId = p.country_id ?? null,
                CountryCode = p.country_code ?? null,
                CountryName = p.country_name ?? null
            });
            return data;
        }

        public void Insert(CountryCbModel Country)
        {
            List<string> column = new List<string>();
            List<string> columnData = new List<string>();
            if (Country.CountryId != null)
            {
                column.Add("coutry_id");
                columnData.Add("@CountryId");
            }
            if (Country.CountryCode != null)
            {
                column.Add("country_code");
                columnData.Add("@CountryCode");
            }
            if (Country.CountryName != null)
            {
                column.Add("country_name");
                columnData.Add("@CountryName");
            }
            if (Country.RegionId != null)
            {
                column.Add("region_id");
                columnData.Add("@RegionId");
            }
            
            string query = "INSERT INTO " + table + "(" + string.Join(", ", column) + ") VALUES (" + string.Join(", ", columnData) + ")";
            _connection.Execute(query, Country);
        }

        public void BulkInsert(List<CountryCbModel> countries)
        {
            if (countries.Count > 0)
            {
                List<string> column = new List<string>();
                if (countries[0].CountryId != null)
                {
                    column.Add("country_id");
                }
                if (countries[0].CountryCode != null)
                {
                    column.Add("country_code");
                }
                if (countries[0].CountryName != null)
                {
                    column.Add("country_name");
                }
                if (countries[0].RegionId != null)
                {
                    column.Add("region_id");
                }
                
                using (var writer = _connection.BeginBinaryImport("COPY " + table + " (" + string.Join(", ", column)+ ") FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var item in countries)
                    {
                        writer.StartRow();
                        if(item.CountryId != null)
                        {
                            writer.Write(item.CountryId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                        if (item.CountryCode != null)
                        {
                            writer.Write(item.CountryCode, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.CountryCode != null)
                        {
                            writer.Write(item.CountryCode, NpgsqlTypes.NpgsqlDbType.Text);
                        }
                        if (item.RegionId != null)
                        {
                            writer.Write(item.RegionId, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                    }

                    writer.Complete();
                }
            }
        }

        public void ReplaceInto(CountryCbModel Country)
        {
            string query = "INSERT INTO " + table;
            List<string> column = new List<string>();
            List<string> dataSet = new List<string>();
            List<string> dataUpdate = new List<string>();
            
            if (Country.CountryCode != null)
            {
                column.Add("country_code");
                dataSet.Add("@CountryCode");
                dataUpdate.Add("country_code = @CountryCode");
            }
            if (Country.CountryName != null)
            {
                column.Add("country_name");
                dataSet.Add("@CountryName");
                dataUpdate.Add("country_name = @CountryName");
            }
            if (Country.CountryId != null)
            {
                column.Add("country_id");
                dataSet.Add("@CountryId");
                dataUpdate.Add("country_id = @CountryId");
            }
            if (Country.RegionId != null)
            {
                column.Add("region_id");
                dataSet.Add("@RegionId");
                dataUpdate.Add("region_id = @RegionId");
            }
            
            query += string.Join(", ", column) + " VALUES (" + string.Join(", ", dataSet) + ") ON CONFLICT (country_id) DO UPDATE SET " + string.Join(", ", dataUpdate);
            _connection.Execute(query, Country);
        }

        public void Update(int id, CountryCbModel Country)
        {
            string query = "UPDATE " + table + " SET ";
            List<string> dataSet = new List<string>();
            if(Country.CountryId != null)
            {
                dataSet.Add("country_id = @NameCountryId");
            }
            if (Country.CountryName != null)
            {
                dataSet.Add("country_name = @CountryName");
            }
            if (Country.CountryCode != null)
            {
                dataSet.Add("country_code = @CountryCode");
            }
            if (Country.RegionId != null)
            {
                dataSet.Add("region_id = @RegionId");
            }
            query += string.Join(", ", dataSet) + " WHERE country_id = @CountryId";
            _connection.Execute(query, Country);
        }

        public void Delete(int id)
        {
            string query = "DELETE FROM " + table + " WHERE country_id = @CountryId";
            _connection.Execute(query, new { CountryId = id } );
        }
    }
}
