using BlingApiDailyConsult.Interfaces;
using BlingApiDailyConsult.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlingApiDailyConsult.Services;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingNotaFiscalFetcher : IBlingApiFetcher<NotaFiscal[]>
    {
        private const string baseUrl = "https://api.bling.com.br/Api/v3/nfe?tipo=0";

        private readonly HttpClientRequestHelper _httpClientrequestHelper;
        private readonly PaginationHelper _paginationHelper;

        public BlingNotaFiscalFetcher(TokenManager tokenManager)
        {
            _httpClientrequestHelper = new HttpClientRequestHelper(tokenManager);
            _paginationHelper = new PaginationHelper();
        }

        public async Task<NotaFiscal[]> ExecuteAsync()
        {
            return (await _paginationHelper.FetchAllPagesAsync<NotaFiscal>(baseUrl, async (paginatedUrl) =>
            {
                // Utilizando o HttpClientHelper para buscar os dados da API
                var apiNfeResponse = await _httpClientrequestHelper.FetchDataAsync<ApiResponse<NotaFiscal>>(paginatedUrl);

                // Retorna a lista de Notas Fiscais deserializada
                return apiNfeResponse?.Data?.ToList() ?? new List<NotaFiscal>();
                
            })).ToArray();
        }
    }
}
