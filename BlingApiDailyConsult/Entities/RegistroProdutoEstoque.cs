using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Entities
{
    public class RegistroProdutoEstoque
    {
        public DateTime? Data { get; set; }
        public string? Entrada { get; set; }
        public string? Saida { get; set; }
        public decimal? PrecoVenda { get; set; }
        public decimal? PrecoCompra { get; set; }
        public decimal? PrecoCusto { get; set; }
        public string? Observacao { get; set; }
        public string? Origem { get; set; }
        public string? Tipo { get; set; }
    }
}
