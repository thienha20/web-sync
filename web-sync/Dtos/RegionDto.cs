namespace web_sync.Dtos
{
    public class RegionDto: BaseDto
    {
        public int? RegionId { get; set; }
        public int[]? RegionIds { get; set; }
        public int? FromRegionId { get; set; }
        public string? RegionName { get; set; }

    }
}
