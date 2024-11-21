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
                string paginetedUrl = $"{baseUrl}&pagina={currentPage}";

                var pageResults = await fetchPageData(paginetedUrl);

                if (pageResults != null && pageResults.Any())
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
