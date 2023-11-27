using System.Security;

namespace web_sync.Dtos
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
