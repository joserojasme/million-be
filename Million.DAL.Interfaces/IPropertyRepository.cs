using Million.DTO.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.DAL.Interfaces
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAllAsync();

        Task<List<Property>> GetFilteredAsync(string name, string address, decimal? minPrice, decimal? maxPrice);
    }
}
