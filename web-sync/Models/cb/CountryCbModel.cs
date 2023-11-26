using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace sync_data.Models.cb
{
    [Table("db_countries")]
    public class CountryCbModel
    {
        [Key]
        [Column("country_id")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CountryId { get; set; }

        [Column("country_code")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CountryCode { get; set; }

        [Column("country_name")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? CountryName { get; set; }

        [Column("region_id")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RegionId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RegionCbModel? Region  { get; set; }
    }
}
