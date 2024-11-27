using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class ApiResponseProduto
    {
        [JsonPropertyName("data")]
        public Produto? Produto { get; set; }
    }
}
