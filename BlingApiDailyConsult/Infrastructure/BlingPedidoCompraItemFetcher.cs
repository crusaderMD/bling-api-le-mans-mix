using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingPedidoCompraItemFetcher : BlingPedidoItemFetcher
    {
        private readonly string _baseUrl = "https://api.bling.com.br/Api/v3/pedidos/compras/";
        private readonly HttpClientRequestHelper _httpClientRequestHelper;
        public BlingPedidoCompraItemFetcher(TokenManager tokenManager) : base(tokenManager)
        {
            _httpClientRequestHelper = new HttpClientRequestHelper(tokenManager) ?? throw new ArgumentNullException(nameof(HttpClientRequestHelper));
        }        
    }
}
