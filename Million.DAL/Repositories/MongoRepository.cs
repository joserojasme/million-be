using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Million.DAL.Context;

namespace Million.DAL.Repositories
{
    public class MongoRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public MongoRepository(MongoDbContext context, string collectionName)
        {
            _collection = context.GetCollection<T>(collectionName);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id, string idField = "Id")
        {
            var filter = Builders<T>.Filter.Eq(idField, id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, T entity, string idField = "Id")
        {
            var filter = Builders<T>.Filter.Eq(idField, id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id, string idField = "Id")
        {
            var filter = Builders<T>.Filter.Eq(idField, id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}
