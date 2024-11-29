using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingPedidoCompraFetcher : IBlingApiFetcher<Pedido[]>
    {
        private const string baseUrl = "https://api.bling.com.br/Api/v3/pedidos/compras?";

        private readonly HttpClientRequestHelper _httpClientHelper;
        private readonly PaginationHelper _paginationHelper;

        public BlingPedidoCompraFetcher(TokenManager tokenManager)
        {
            _httpClientHelper = new HttpClientRequestHelper(tokenManager);
            _paginationHelper = new PaginationHelper();
        }
        public async Task<Pedido[]> ExecuteAsync()
        {
            return (await _paginationHelper.FetchAllPagesAsync<Pedido>(baseUrl, async (paginatedUrl) =>
            {
                // Utilizando o HttpClientHelper para buscar os dados da API
                var apiPedidoCompraResponse = await _httpClientHelper.FetchDataAsync<ApiResponse<Pedido>>(paginatedUrl);

                // Retorna a lista de pedidos deserializada
                return apiPedidoCompraResponse?.Data?.ToList() ?? new List<Pedido>();
            })).ToArray();
        }
    }
}
