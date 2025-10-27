using System.Collections.Generic;
using System.Threading.Tasks;
using Million.DTO.Entities;

namespace Million.Application.Interfaces
{
    using Million.DTO.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPropertyService
    {
        Task<List<Property>> GetAllAsync();
        Task<List<Property>> GetFilteredAsync(string name, string address, decimal? minPrice, decimal? maxPrice);
    }
}
