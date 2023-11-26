using sync_data.Models.cb;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace sync_data.Models.ob
{
    public class UserObModel
    {
        [Key]
        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("username")]
        public string? UserName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("country_id")]
        public int? CountryId { get; set; }
        public CountryCbModel? Country { get; set; }
        public List<PostCbModel>? Posts { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

    }
}
