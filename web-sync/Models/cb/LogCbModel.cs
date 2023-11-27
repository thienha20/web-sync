using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_sync.Models.cb
{
    public class LogCbModel
    {
        [Key]
        [Column("log_id")]
        public long? LogId { get; set; }

        [Column("object_name")]
        public string? ObjectName { get; set; }

        [Column("object_type")]
        public string? ObjectType { get; set; }

        [Column("object_id")]
        public long? ObjectId { get; set; }

        [Column("timestamps")]
        public DateTime? Timestamps { get; set; }
    }
}
