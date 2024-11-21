using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Entities
{
    public class PedidoResponse
    {
        [JsonPropertyName("data")]
        public List<Pedido>? Pedidos { get; set; }
    }
}
