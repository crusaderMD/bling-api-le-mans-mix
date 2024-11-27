using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class NotaFiscal
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
