using Million.DAL.Interfaces;
using Million.DTO.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.DAL.Repositories
{
    public class PropertyTraceRepository : IPropertyTraceRepository
    {
        private readonly IMongoCollection<PropertyTrace> _collection;

        public PropertyTraceRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<PropertyTrace>("PropertyTrace");
        }

        public async Task<List<PropertyTrace>> GetByPropertyIdAsync(string propertyId)
        {
            return await _collection.Find(pt => pt.IdProperty == propertyId).ToListAsync();
        }
    }
}
