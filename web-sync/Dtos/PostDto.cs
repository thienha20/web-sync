namespace web_sync.Dtos
{
    public class PostDto: BaseDto
    {
        public long? PostId { get; set; }
        public long? FromPostId { get; set; }
        public long[]? PostIds { get; set; }
        public string? Name { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public long? UserId { get; set; }
        public long? CategoryId { get; set; }

    }
}
