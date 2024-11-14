using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Entities
{
    public class Contato
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("tipoPessoa")]
        public string TipoPessoa { get; set; }

        [JsonPropertyName("numeroDocumento")]
        public string NumeroDocumento { get; set; }
    }
}
