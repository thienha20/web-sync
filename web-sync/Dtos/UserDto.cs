namespace web_sync.Dtos
{
    public class UserDto: BaseDto
    {
        public long? UserId { get; set; }
        public long? FromUserId { get; set; }
        public long[]? UserIds { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public long? CountryId { get; set; }

    }
}
