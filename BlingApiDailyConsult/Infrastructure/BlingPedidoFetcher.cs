using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Services;

// Namespace responsável pelas interações com a API Bling e o banco de dados
namespace BlingApiDailyConsult.Infrastructure
{
    // Classe responsável por buscar os pedidos da API Bling e armazená-los no banco de dados
    internal class BlingPedidoFetcher : IBlingApiFetcher<Pedido[]>
    {
        // URL da API Bling com os parâmetros para consulta de pedidos
        private const string baseUrl = "https://api.bling.com.br/Api/v3/pedidos/vendas?";

        private readonly HttpClientRequestHelper _httpClientRequestHelper;
        private readonly DateRequestHelper _dateRequestHelper;
        private readonly PaginationHelper _paginationHelper;       

        public BlingPedidoFetcher(TokenManager tokenManager)
        {
            _httpClientRequestHelper = new HttpClientRequestHelper(tokenManager);
            _dateRequestHelper = new DateRequestHelper();
            _paginationHelper = new PaginationHelper();
        }

        // Método para obter pedidos da API Bling
        public async Task<Pedido[]> ExecuteAsync()
        {
            string dateFilteredUrl = baseUrl + _dateRequestHelper.GetDateQueryString();

            return (await _paginationHelper.FetchAllPagesAsync<Pedido>(dateFilteredUrl, async (paginatedUrl) =>
            {
                // Utilize o HttpClientHelper para buscar os dados da API                 
                var apiPedidoResponse = await _httpClientRequestHelper.FetchDataAsync<ApiResponse<Pedido>>(paginatedUrl);
                
                // Retorna a lista de pedidos deserializada
                return apiPedidoResponse?.Data?.ToList() ?? new List<Pedido>();               
            })).ToArray();
        }        
    }
}
