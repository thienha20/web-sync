namespace sync_data.Dtos
{
    public class PostDto: BaseDto
    {
        public int[]? PostId { get; set; }
        public string? Name { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }

    }
}
