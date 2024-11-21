using BlingApiDailyConsult.Interfaces;

namespace BlingApiDailyConsult.Services
{
    public class BlingApiFetchService<T>
    {
        private readonly IEnumerable<IBlingApiFetcher<T>> _fetchers;

        public BlingApiFetchService(IEnumerable<IBlingApiFetcher<T>> fetchers)
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
