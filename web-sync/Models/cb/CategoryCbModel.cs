using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sync_data.Models.cb
{
    public class CategoryCbModel
    {
        [Key]
        [Column("category_id")]
        public int? CategoryId { get; set; }

        [Column("parent_id")]
        public int? ParentId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("path")]
        public string? Path { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public List<PostCbModel>? Posts { get; set; }
    }
}
