using System.Text.Json.Serialization;

namespace BlingApiDailyConsult.Entities
{
    public class Item
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("codigo")]
        public string? Codigo { get; set; }

        [JsonPropertyName("unidade")]
        public string? Unidade { get; set; }

        [JsonPropertyName("quantidade")]
        public decimal Quantidade { get; set; }

        [JsonPropertyName("desconto")]
        public decimal Desconto { get; set; }

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }

        [JsonPropertyName("aliquotaIPI")]
        public decimal AliquotaIPI { get; set; }

        [JsonPropertyName("descricao")]
        public string? Descricao { get; set; }

        [JsonPropertyName("descricaoDetalhada")]
        public string? DescricaoDetalhada { get; set; }

        [JsonPropertyName("produto")]
        public Produto? Produto { get; set; }

        [JsonPropertyName("comissao")]
        public Comissao? Comissao { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Nome: {Descricao} Codigo: {Codigo} Quantidade: {Quantidade}";
        }
    }
}
