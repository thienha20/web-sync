namespace web_sync.Dtos
{
    public class PostDto: BaseDto
    {
        public int? PostId { get; set; }
        public int? FromPostId { get; set; }
        public int[]? PostIds { get; set; }
        public string? Name { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsUpdate { get; set; } = false;
        public DateTime? UpdatedDateFrom { get; set; }

    }
}
