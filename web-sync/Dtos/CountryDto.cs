namespace sync_data.Dtos
{
    public class CountryDto: BaseDto
    {
        public int? CountryId { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public int? RegionId { get; set; }

    }
}
