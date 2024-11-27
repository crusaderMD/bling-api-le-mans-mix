using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class Desconto
    {
        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        [JsonPropertyName("unidade")]
        public string? Unidade { get; set; }
    }
}
