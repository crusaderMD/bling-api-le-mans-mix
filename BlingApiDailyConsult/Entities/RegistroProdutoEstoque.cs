using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Entities
{
    public class RegistroProdutoEstoque
    {
        public string? Data { get; set; }
        public string? Entrada { get; set; }
        public string? Saida { get; set; }
        public string? PrecoVenda { get; set; }
        public string? PrecoCompra { get; set; }
        public string? PrecoCusto { get; set; }
        public string? Observacao { get; set; }
        public string? Origem { get; set; }
        public string? Tipo { get; set; }
    }
}
