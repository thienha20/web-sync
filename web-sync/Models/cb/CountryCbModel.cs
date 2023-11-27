using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_sync.Models.cb
{
    [Table("db_countries")]
    public class CountryCbModel
    {
        [Key]
        [Column("country_id")]
        public long? CountryId { get; set; }

        [Column("country_code")]
        public string? CountryCode { get; set; }

        [Column("country_name")]
        public string? CountryName { get; set; }

        [Column("region_id")]
        public long? RegionId { get; set; }
    
        public RegionCbModel? Region  { get; set; }
    }
}
