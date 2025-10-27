using Million.DAL.Interfaces;
using Million.DTO.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.DAL.Repositories
{
    public class PropertyImageRepository : IPropertyImageRepository
    {
        private readonly IMongoCollection<PropertyImage> _collection;

        public PropertyImageRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<PropertyImage>("PropertyImage");
        }

        public async Task<List<PropertyImage>> GetByPropertyIdAsync(string propertyId)
        {
            return await _collection.Find(pi => pi.IdProperty == propertyId).ToListAsync();
        }
    }
}
