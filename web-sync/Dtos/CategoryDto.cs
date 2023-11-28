namespace web_sync.Dtos
{
    public class CategoryDto: BaseDto
    {
        public int? CategoryId { get; set; }
        public int? FromCategoryId { get; set; }
        public int[]? CategoryIds { get; set; }
        public int? ParentId { get; set; }
        public string? Name { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public bool? IsUpdate { get; set; } = false;
        public DateTime? UpdatedDateFrom { get; set; }

    }
}
