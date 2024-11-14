using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Entities
{
    public class Loja
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
