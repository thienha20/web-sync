using web_sync.Models.cb;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_sync.Models.ob
{
    public class RegionObModel
    {
        [Key]
        [Column("region_id")]
        public long? RegionId { get; set; }

        [Column("region_name")]
        public string? RegionName { get; set; }
        public List<CountryCbModel>? Countries { get; set; }
    }
}
