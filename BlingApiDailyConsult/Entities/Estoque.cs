using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class Estoque
    {
        [JsonPropertyName("saldoVirtualTotal")]
        public float SaldoTotal { get; set; }
    }
}