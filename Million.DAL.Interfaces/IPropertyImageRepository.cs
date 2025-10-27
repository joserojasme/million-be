using Million.DTO.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.DAL.Interfaces
{
    public interface IPropertyImageRepository
    {
        Task<List<PropertyImage>> GetByPropertyIdAsync(string propertyId);
    }
}
