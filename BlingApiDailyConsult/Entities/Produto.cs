using System.Text.Json.Serialization;


namespace BlingApiDailyConsult.Entities
{
    public class Produto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }

        [JsonPropertyName("precoCusto")]
        public decimal PrecoCusto { get; set; }

        [JsonPropertyName("estoque")]
        public Estoque? Estoque { get; set; }

        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("situacao")]
        public string? Situacao { get; set; }

        [JsonPropertyName("formato")]
        public string? Formato { get; set; }

        [JsonPropertyName("fornecedor")]
        public Fornecedor? Fornecedor { get; set; }
    }
}
