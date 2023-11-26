namespace sync_data.Dtos
{
    public class LogDto: BaseDto
    {
        public int? LogId { get; set; }
        public string? ObjectName { get; set; }
        public string? ObjectType { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public int? ObjectId { get; set; }

    }
}
