using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sync_data.Models.cb
{
    public class LogCbModel
    {
        [Key]
        [Column("log_id")]
        public int? LogId { get; set; }

        [Column("object_name")]
        public string? ObjectName { get; set; } = string.Empty;

        [Column("object_type")]
        public string? ObjectType { get; set; } = string.Empty;

        [Column("object_id")]
        public int? ObjectId { get; set; }

        [Column("timestamps")]
        public DateTime? Timestamps { get; set; }
    }
}
