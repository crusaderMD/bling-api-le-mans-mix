using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class ApiResponsePedido
    {
        [JsonPropertyName("data")]
        public Pedido? Pedido { get; set; }
    }
}
