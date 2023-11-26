namespace sync_data.Dtos
{
    public class CategoryDto: BaseDto
    {
        public int? CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string? Name { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }

    }
}
