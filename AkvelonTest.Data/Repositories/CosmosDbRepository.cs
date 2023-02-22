using Microsoft.Azure.Cosmos;

namespace AkvelonTest
{
    public class CosmosDbRepository<T> : ICosmosDbRepository<T> where T : Entity<string>
    {
        private readonly Container _container;

        public CosmosDbRepository(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task<T> AddItemAsync(T item)
        {
            try
            {
                var response = await _container.CreateItemAsync(item, new PartitionKey(item.PartitionId));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return item;
            }
        }

        public async Task DeleteItemAsync(string id)
        {
            await _container.DeleteItemAsync<T>(id, new PartitionKey(1));
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<T>(id, new PartitionKey(1));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }

        }

        public async Task UpdateItemAsync(string id, T item)
        {
            await _container.UpsertItemAsync(item, new PartitionKey(item.PartitionId));
        }
    }
}
