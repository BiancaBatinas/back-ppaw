using System.Collections.Generic;
using System.Threading.Tasks;

using ppawproject.Database.Entities;
using ppawproject.Models;

namespace ppawproject.Interfaces
{
    public interface IMarketplaceService
    {
        Task<IEnumerable<object>> GetMarketplacesByUser(int userId);
        Task<Marketplace> AddMarketplace(Marketplace marketplace);
        Task<Marketplace> UpdateMarketplace(int id, UpdateMarketplaceDTO updatedMarketplace);
        Task<bool> DeleteMarketplace(int id);
    }
}
