using web_sync.Models.cb;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace web_sync.Models.ob
{
    public class UserObModel
    {
        [Key]
        [Column("user_id")]
        public long? UserId { get; set; }

        [Column("username")]
        public string? UserName { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("full_name")]
        public string? FullName { get; set; }

        [Column("country_id")]
        public long? CountryId { get; set; }
        public CountryCbModel? Country { get; set; }
        public List<PostCbModel>? Posts { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

    }
}
