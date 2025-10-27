using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Million.DAL.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration["DatabaseSettings:ConnectionString"];
            var databaseName = configuration["DatabaseSettings:DatabaseName"];

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
