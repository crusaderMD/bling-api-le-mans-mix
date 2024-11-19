using BlingApiDailyConsult.Interfaces;

namespace BlingApiDailyConsult.Services
{
    public class BlingApiFetchService
    {
        private readonly IEnumerable<IBlingApiFetcher> _fetchers;

        public BlingApiFetchService(IEnumerable<IBlingApiFetcher> fetchers)
        {
            _fetchers = fetchers;
        }

        public async Task FetchAllAsync()
        {
            foreach (var fetcher in _fetchers)
            {
                await fetcher.ExecuteAsync();
            }
        }
    }
}
