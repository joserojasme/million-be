using Million.DAL.Interfaces;
using Million.DTO.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Million.DAL.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _collection;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IPropertyTraceRepository _propertyTraceRepository;

        public PropertyRepository(IMongoDatabase database, IOwnerRepository ownerRepository, IPropertyImageRepository propertyImageRepository, IPropertyTraceRepository propertyTraceRepository)
        {
            _collection = database.GetCollection<Property>("Property");
            _ownerRepository = ownerRepository;
            _propertyImageRepository = propertyImageRepository;
            _propertyTraceRepository = propertyTraceRepository;
        }

        public async Task<List<Property>> GetAllAsync()
        {
            var properties = await _collection.Find(_ => true).ToListAsync();
            await LoadRelationsAsync(properties);
            return properties;
        }

        public async Task<List<Property>> GetFilteredAsync(string name, string address, decimal? minPrice, decimal? maxPrice)
        {
            var filterBuilder = Builders<Property>.Filter;
            var filters = new List<FilterDefinition<Property>>();

            if (!string.IsNullOrEmpty(name))
                filters.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i")));

            if (!string.IsNullOrEmpty(address))
                filters.Add(filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(address, "i")));

            if (minPrice.HasValue)
                filters.Add(filterBuilder.Gte(p => p.Price, minPrice.Value));

            if (maxPrice.HasValue)
                filters.Add(filterBuilder.Lte(p => p.Price, maxPrice.Value));

            var filter = filters.Count > 0
                ? filterBuilder.And(filters)
                : filterBuilder.Empty;

            var properties = await _collection.Find(filter).ToListAsync();
            await LoadRelationsAsync(properties);
            return properties;
        }

        private async Task LoadRelationsAsync(List<Property> properties)
        {
            if (!properties.Any()) return;

            // Obtener todos los IDs únicos para hacer consultas en lote
            var ownerIds = properties.Select(p => p.OwnerId).Distinct().ToList();
            var propertyIds = properties.Select(p => p.Id).ToList();

            // Cargar todos los owners de una vez
            var owners = new Dictionary<string, Owner>();
            foreach (var ownerId in ownerIds)
            {
                var owner = await _ownerRepository.GetByIdAsync(ownerId);
                if (owner != null)
                {
                    owners[ownerId] = owner;
                }
            }

            // Cargar todas las imágenes de una vez
            var allImages = new Dictionary<string, List<PropertyImage>>();
            foreach (var propertyId in propertyIds)
            {
                var images = await _propertyImageRepository.GetByPropertyIdAsync(propertyId);
                allImages[propertyId] = images;
            }

            // Cargar todos los traces de una vez
            var allTraces = new Dictionary<string, List<PropertyTrace>>();
            foreach (var propertyId in propertyIds)
            {
                var traces = await _propertyTraceRepository.GetByPropertyIdAsync(propertyId);
                allTraces[propertyId] = traces;
            }

            // Asignar las relaciones a cada property
            foreach (var property in properties)
            {
                if (owners.ContainsKey(property.OwnerId))
                {
                    property.Owner = owners[property.OwnerId];
                }

                if (allImages.ContainsKey(property.Id))
                {
                    property.PropertyImages = allImages[property.Id];
                }

                if (allTraces.ContainsKey(property.Id))
                {
                    property.PropertyTraces = allTraces[property.Id];
                }
            }
        }
    }
}
