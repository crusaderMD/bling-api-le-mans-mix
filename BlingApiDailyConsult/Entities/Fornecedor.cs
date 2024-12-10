using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Entities
{
    public class Fornecedor
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("situacao")]
        public string? Situacao { get; set; }

        [JsonPropertyName("numeroDocumento")]
        public string? NumeroDocumento { get; set; }

        [JsonPropertyName("fantasia")]
        public string? NomeFantasia { get; set; }

        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("contato")]
        public Contato? Contato { get; set; }
    }
}
