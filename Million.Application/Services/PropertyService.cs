using Million.Application.Interfaces;
using Million.DAL.Interfaces;
using Million.DTO.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _repository;

        public PropertyService(IPropertyRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Property>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<Property>> GetFilteredAsync(string name, string address, decimal? minPrice, decimal? maxPrice)
        {
            return await _repository.GetFilteredAsync(name, address, minPrice, maxPrice);
        }
    }
}
