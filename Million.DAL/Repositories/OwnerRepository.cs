using Million.DAL.Interfaces;
using Million.DTO.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Million.DAL.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IMongoCollection<Owner> _collection;

        public OwnerRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Owner>("Owner");
        }

        public async Task<Owner> GetByIdAsync(string id)
        {
            return await _collection.Find(o => o.IdOwner == id).FirstOrDefaultAsync();
        }
    }
}
