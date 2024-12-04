using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class NotaFiscal
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("tipo")]
        public int Tipo { get; set; }

        [JsonPropertyName("situacao")]
        public Situacao? Situacao { get; set; }

        [JsonPropertyName("numero")]
        public string? Numero { get; set; }

        [JsonPropertyName("dataEmissao")]
        public string? DataEmissao { get; set; }

        [JsonPropertyName("dataOperacao")]
        public string? DataOperacao { get; set; }

        [JsonPropertyName("contato")]
        public Fornecedor? Fornecedor { get; set; }

        [JsonPropertyName("serie")]
        public int Serie { get; set; }

        [JsonPropertyName("valorNota")]
        public decimal ValorNota { get; set; }

        [JsonPropertyName("chaveAcesso")]
        public string? ChaveAcesso { get; set; }

        [JsonPropertyName("xml")]
        public string? LinkXml { get; set; }



    }
}
