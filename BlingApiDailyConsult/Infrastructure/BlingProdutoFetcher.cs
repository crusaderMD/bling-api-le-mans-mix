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
    internal class BlingProdutoFetcher : IBlingApiFetcher<Produto[]>
    {
        // URL da API Bling com os parâmetros para consulta de produtos
        private const string baseUrl = "https://api.bling.com.br/Api/v3/produtos?";

        private readonly HttpClientRequestHelper _httpClientHelper;        
        private readonly PaginationHelper _paginationHelper;

        public BlingProdutoFetcher(TokenManager tokenManager)
        {
            _httpClientHelper = new HttpClientRequestHelper(tokenManager);            
            _paginationHelper = new PaginationHelper();
        }

        public async Task<Produto[]> ExecuteAsync()
        {          
            return (await _paginationHelper.FetchAllPagesAsync<Produto>(baseUrl, async (paginatedUrl) =>
            {
                var apiProdutoResponse = await _httpClientHelper.FetchDataAsync<ApiResponse<Produto>>(paginatedUrl);

                return apiProdutoResponse?.Data?.ToList() ?? new List<Produto>();
            }

            )).ToArray();
        }
    }
}
