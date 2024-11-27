using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class Comissao
    {
        [JsonPropertyName("base")]
        public decimal Base { get; set; }

        [JsonPropertyName("aliquota")]
        public decimal Aliquota { get; set; }

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }
}
