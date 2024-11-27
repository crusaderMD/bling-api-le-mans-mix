using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class Categoria
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }
    }
}
