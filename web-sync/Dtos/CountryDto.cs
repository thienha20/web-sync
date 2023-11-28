namespace web_sync.Dtos
{
    public class CountryDto: BaseDto
    {
        public int? CountryId { get; set; }
        public int[]? CountryIds { get; set; }
        public int? FromCountryId { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public int? RegionId { get; set; }

    }
}
