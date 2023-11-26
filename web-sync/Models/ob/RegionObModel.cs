using sync_data.Models.cb;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sync_data.Models.ob
{
    public class RegionObModel
    {
        [Key]
        [Column("region_id")]
        public int? RegionId { get; set; }

        [Column("region_name")]
        public string? RegionName { get; set; }
        public List<CountryCbModel>? Countries { get; set; }
    }
}
