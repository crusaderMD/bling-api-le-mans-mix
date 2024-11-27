using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class Tributacao
    {
        [JsonPropertyName("totalICMS")]
        public decimal TotalICMS { get; set; }

        [JsonPropertyName("totalIPI")]
        public decimal TotalIPI { get; set; }
    }
}
