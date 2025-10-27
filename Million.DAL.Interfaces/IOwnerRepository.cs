using Million.DTO.Entities;
using System.Threading.Tasks;

namespace Million.DAL.Interfaces
{
    public interface IOwnerRepository
    {
        Task<Owner> GetByIdAsync(string id);
    }
}
