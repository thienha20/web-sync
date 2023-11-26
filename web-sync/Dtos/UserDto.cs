namespace sync_data.Dtos
{
    public class UserDto: BaseDto
    {
        public int? UserId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public int? CountryId { get; set; }

    }
}
