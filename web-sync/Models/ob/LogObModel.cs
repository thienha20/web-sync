using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_sync.Models.ob
{
    public class LogObModel
    {
        [Key]
        [Column("log_id")]
        public long? LogId { get; set; }

        [Column("object_name")]
        public string? ObjectName { get; set; } = string.Empty;

        [Column("object_type")]
        public string? ObjectType { get; set; } = string.Empty;

        [Column("object_id")]
        public long? ObjectId { get; set; }

        [Column("timestamps")]
        public DateTime? Timestamps { get; set; }
    }
}
