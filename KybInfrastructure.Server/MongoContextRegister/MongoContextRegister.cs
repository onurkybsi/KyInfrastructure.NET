using KybInfrastructure.Core;
using KybInfrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace KybInfrastructure.Server
{
    /// <summary>
    /// Contains MongoContext registration strategies
    /// </summary>
    public static class MongoContextRegister
    {
        /// <summary>
        /// Set MongoContext objects lifetime via IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="mongoDbConnectionString">Connection string of MongoDb server</param>
        /// <param name="databaseName">Database name that is requested connection</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoContext(this IServiceCollection services, string mongoDbConnectionString, string databaseName)
        {
            ValidateMongoContextConfigurationValues(mongoDbConnectionString, databaseName);

            services.AddSingleton<IMongoClient>((serviceProvider) =>
            {
                MongoClientSettings settings = MongoClientSettings.FromConnectionString(mongoDbConnectionString);
                return new MongoClient(settings);
            });
            services.AddScoped((serviceProvider) =>
            {
                IMongoClient client = serviceProvider.GetRequiredService<IMongoClient>();
                return new MongoContext(client.GetDatabase(databaseName));
            });

            return services;
        }

        private static void ValidateMongoContextConfigurationValues(string mongoDbConnectionString, string databaseName)
        {
            if (string.IsNullOrWhiteSpace(mongoDbConnectionString))
                throw new InvalidArgumentException(nameof(mongoDbConnectionString), mongoDbConnectionString);
            if (string.IsNullOrWhiteSpace(databaseName))
                throw new InvalidArgumentException(nameof(databaseName), databaseName);
        }
    }
}
