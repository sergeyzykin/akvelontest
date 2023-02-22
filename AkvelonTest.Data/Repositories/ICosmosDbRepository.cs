namespace AkvelonTest
{
    public interface ICosmosDbRepository<T> where T : Entity<string>
    {
        Task<T> GetItemAsync(string id);
        Task<T> AddItemAsync(T item);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
