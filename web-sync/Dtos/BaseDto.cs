using System.Security;

namespace sync_data.Dtos
{
    public class BaseDto
    {
        public int? Limit { get; set; }  
        public int? Offset { get; set; }
        public string? SortOrder { get; set; }
        public string? SortBy { get; set; }
        public string[]? Fields { get; set; }
    }
}
