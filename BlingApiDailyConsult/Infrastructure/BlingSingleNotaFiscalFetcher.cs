using BlingApiDailyConsult.Entities;
using BlingApiDailyConsult.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Infrastructure
{
    internal class BlingSingleNotaFiscalFetcher : IBlingApiFetcher<NotaFiscal>
    {
        private const string baseUrl = "https://api.bling.com.br/Api/v3/nfe/";

        private readonly HttpClientRequestHelper _httpClientrequestHelper;

        public BlingSingleNotaFiscalFetcher(TokenManager tokenManager)
        {
            _httpClientrequestHelper = new HttpClientRequestHelper(tokenManager);
        }

        public Task<NotaFiscal> ExecuteAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<NotaFiscal> GetSingleNotaFiscal(long NotaFiscalId)
        {
            string url = baseUrl + NotaFiscalId.ToString();

            // apagar depois
            Console.WriteLine(this + " retorna: " + url);

            var apiNotaFiscalResponse = await _httpClientrequestHelper.FetchDataAsync<ApiSingleResponse<NotaFiscal>>(url);

            // apagar depois
            Console.WriteLine(apiNotaFiscalResponse.ToString());

            return apiNotaFiscalResponse.Data;
        }
    }
}
