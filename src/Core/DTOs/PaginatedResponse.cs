using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.DTOs
{
    public class PaginatedResponse<T>
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = "success";

        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; } = new List<T>();

        [JsonPropertyName("pagination")]
        public PaginationMeta Pagination { get; set; } = new PaginationMeta();
    }

    public class PaginationMeta
    {
        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }
    }
}