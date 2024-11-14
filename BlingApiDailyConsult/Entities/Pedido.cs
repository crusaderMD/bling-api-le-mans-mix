using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class Pedido
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("numero")]
        public int Numero { get; set; }

        [JsonPropertyName("numeroLoja")]
        public string NumeroLoja { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("dataSaida")]
        public string DataSaida { get; set; }

        [JsonPropertyName("dataPrevista")]
        public string DataPrevista { get; set; }

        [JsonPropertyName("totalProdutos")]
        public decimal TotalProdutos { get; set; }

        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        [JsonPropertyName("contato")]
        public Contato Contato { get; set; }

        [JsonPropertyName("situacao")]
        public Situacao Situacao { get; set; }

        [JsonPropertyName("loja")]
        public Loja Loja { get; set; }
    }
}
