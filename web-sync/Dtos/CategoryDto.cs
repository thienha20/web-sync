namespace web_sync.Dtos
{
    public class CategoryDto: BaseDto
    {
        public long? CategoryId { get; set; }
        public long? FromCategoryId { get; set; }
        public long[]? CategoryIds { get; set; }
        public long? ParentId { get; set; }
        public string? Name { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public bool? IsUpdate { get; set; } = false;
        public DateTime? UpdatedDateFrom { get; set; }

    }
}
