namespace web_sync.Dtos
{
    public class CountryDto: BaseDto
    {
        public long? CountryId { get; set; }
        public long[]? CountryIds { get; set; }
        public long? FromCountryId { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public long? RegionId { get; set; }

    }
}
