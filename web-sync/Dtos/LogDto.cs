namespace web_sync.Dtos
{
    public class LogDto: BaseDto
    {
        public long? LogId { get; set; }
        public long? FromLogId { get; set; }
        public string? ObjectName { get; set; }
        public string[]? ObjectNames { get; set; }
        public string? ObjectType { get; set; }
        public string[]? ObjectTypes { get; set; }
        public string? CreatedDateFrom { get; set; }
        public string? CreatedDateTo { get; set; }
        public long? ObjectId { get; set; }

    }
}
