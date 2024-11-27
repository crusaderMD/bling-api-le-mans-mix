using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using System.Threading.Tasks;


namespace BlingApiDailyConsult.Infrastructure
{
    class BlingSingleProdutoFetcher
    {
        private const string baseUrl = "https://api.bling.com.br/Api/v3/produtos/";

        private readonly HttpClientRequestHelper _httpClientRequestHelper;
        public BlingSingleProdutoFetcher(TokenManager tokenManager)
        {
            _httpClientRequestHelper = new HttpClientRequestHelper(tokenManager);
        }

        public async Task<Produto> GetSingleProduto(long produtoId)
        {
            string url = baseUrl + produtoId.ToString();

            //apagar depois
            Console.WriteLine(this + " retorna: " + url);

            var apiProdutoResponse = await _httpClientRequestHelper.FetchDataAsync<ApiResponseProduto>(url);

            return apiProdutoResponse.Produto;
        }       
    }
}
