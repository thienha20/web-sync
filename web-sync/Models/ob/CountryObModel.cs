using sync_data.Models.cb;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sync_data.Models.ob
{
    public class CountryObModel
    {
        [Key]
        [Column("country_id")]
        public int? CountryId { get; set; }

        [Column("country_code")]
        public string? CountryCode { get; set; }

        [Column("country_name")]
        public string? CountryName { get; set; }

        [Column("region_id")]
        public int? RegionId { get; set; }
        public RegionCbModel? Region { get; set; }
    }
}
