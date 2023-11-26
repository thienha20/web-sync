using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sync_data.Models.cb
{
    public class PostCbModel
    {
        [Key]
        [Column("post_id")]
        public int? PostId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }
        public UserCbModel? User { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }
        public CategoryCbModel? Category { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt  { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt  { get; set; }
    }
}
