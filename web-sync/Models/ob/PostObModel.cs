using web_sync.Models.cb;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_sync.Models.ob
{
    public class PostObModel
    {
        [Key]
        [Column("post_id")]
        public long? PostId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("user_id")]
        public long? UserId { get; set; }
        public UserCbModel? User { get; set; }

        [Column("category_id")]
        public long? CategoryId { get; set; }
        public CategoryCbModel? Category { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
