using System.Security.Policy;

namespace BlingApiDailyConsult.Services
{
    public class PaginationHelper
    {
        public async Task<List<T>> FetchAllPagesAsync<T>(string baseUrl, Func<string, Task<List<T>>> fetchPageData)
        {
            var allResults = new List<T>();
            int currentPage = 1;
            bool hasMorePages = true;

            while (hasMorePages)
            {
                string paginetedUrl = $"{baseUrl}&pagina={currentPage}&limite=100";

                Console.WriteLine($"Requisitando URL: {paginetedUrl}");

                var pageResults = await fetchPageData(paginetedUrl);

                await Task.Delay(500); // Pausa de 1 segundo antes de buscar a próxima página                

                if (pageResults != null && pageResults.Count != 0)
                {
                    allResults.AddRange(pageResults);
                    currentPage++;
                }
                else
                {
                    hasMorePages = false;
                }               
            }
            return allResults;
        }
    }
}
