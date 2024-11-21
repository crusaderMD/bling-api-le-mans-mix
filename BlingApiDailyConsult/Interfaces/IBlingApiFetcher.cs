namespace BlingApiDailyConsult.Interfaces
{
    public interface IBlingApiFetcher<T>
    {
        Task<T> ExecuteAsync();
    }
}
