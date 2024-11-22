using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("data")]
        public List<T>? Data { get; set; }
    }
}
