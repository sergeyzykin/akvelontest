using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AkvelonTest
{
    public static class AkvelonTestDataExtensions
    {
        public static IServiceCollection AddCosmosDbConnection(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ICosmosDbRepository<ImageTransform>>(
                options =>
                {
                    var database = config["Cosmos:Database"];
                    var container = config["Cosmos:CollectionName"];
                    var connectionString = config["Cosmos:ConnectionString"];
                    Microsoft.Azure.Cosmos.CosmosClient client;

                    client = new Microsoft.Azure.Cosmos.CosmosClient(connectionString: connectionString);
                    var service = new CosmosDbRepository<ImageTransform>(client, database, container);

                    return service;
                }
            );

            return services;
        }

        public static IServiceCollection AddBlobStorageConnection(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IStorageService>(
                options =>
                {
                    var connectionString = config["BlobStorage:ConnectionString"];
                    var containerName = config["BlobStorage:ContainerName"];

                    var client = new Azure.Storage.Blobs.BlobContainerClient(connectionString, containerName);
                    var service = new StorageService(client);

                    return service;
                }
            );

            return services;
        }

        public static IServiceCollection AddServiceBusConnection(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IMessageBroker>(
                options =>
                {
                    var connectionString = config["MessageBroker:ConnectionString"];
                    var topicName = config["MessageBroker:TopicName"];

                    var client = new Azure.Messaging.ServiceBus.ServiceBusClient(connectionString);
                    var service = new MessageBroker(client.CreateSender(topicName));

                    return service;
                }
            );

            return services;
        }
    }
}
