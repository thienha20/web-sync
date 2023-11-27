namespace web_sync.Dtos
{
    public class RegionDto: BaseDto
    {
        public long? RegionId { get; set; }
        public long[]? RegionIds { get; set; }
        public long? FromRegionId { get; set; }
        public string? RegionName { get; set; }

    }
}
