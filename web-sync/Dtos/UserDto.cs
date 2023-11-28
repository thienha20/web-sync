namespace web_sync.Dtos
{
    public class UserDto: BaseDto
    {
        public int? UserId { get; set; }
        public int? FromUserId { get; set; }
        public int[]? UserIds { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public int? CountryId { get; set; }

        public bool? IsUpdate { get; set; } = false;
        public DateTime? UpdatedDateFrom { get; set; }

    }
}
