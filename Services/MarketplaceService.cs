using Microsoft.EntityFrameworkCore;

using ppawproject.Database;
using ppawproject.Database.Entities;
using ppawproject.Interfaces;
using ppawproject.Models;

namespace ppawproject.Services
{
    public class MarketplaceService : IMarketplaceService
    {
        private readonly DatabaseContext _context;

        public MarketplaceService(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<object>> GetMarketplacesByUser(int userId)
        {
            try
            {
                var marketplaces = await _context.Marketplaces
                    .Where(m => m.UserId == userId)
                    .Select(m => new
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Link = m.Link,
                        Image = m.Image != null ? Convert.ToBase64String(m.Image) : null,
                        PriceModifier = m.PriceModifier,
                    })
                    .ToListAsync();

                return marketplaces;
            }
            catch (Exception ex)
            {
                // Logare eroare
                // _logger.LogError(ex, "A apărut o eroare la obținerea marketplace-urilor.");
                throw new Exception("A apărut o eroare la obținerea marketplace-urilor.", ex);
            }
        }

        public async Task<Marketplace> AddMarketplace(Marketplace marketplace)
        {
            _context.Marketplaces.Add(marketplace);
            await _context.SaveChangesAsync();
            return marketplace;
        }

        public async Task<Marketplace> UpdateMarketplace(int id, UpdateMarketplaceDTO updatedMarketplace)
        {
            var marketplace = await _context.Marketplaces.FindAsync(updatedMarketplace.Id);

            if (marketplace == null)
                throw new Exception("Marketplace-ul nu a fost găsit.");

            marketplace.Name = updatedMarketplace.Name;
            marketplace.Link = updatedMarketplace.Link;
            marketplace.Image = updatedMarketplace.Image;
            marketplace.PriceModifier = updatedMarketplace.PriceModifier;

            await _context.SaveChangesAsync();
            return marketplace;
        }

        public async Task<bool> DeleteMarketplace(int id)
        {
            var marketplace = await _context.Marketplaces.FindAsync(id);

            if (marketplace == null)
                return false;

            _context.Marketplaces.Remove(marketplace);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
