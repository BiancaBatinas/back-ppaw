using Microsoft.EntityFrameworkCore;

using ppawproject.Database;
using ppawproject.Database.Entities;
using ppawproject.Interfaces;
using ppawproject.Models;

namespace ppawproject.Services
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;

        public UserService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<int, List<ProductDTO>>> GetUserProductsWithMarketplaces(int userId)
        {
            var userExists = await _context.Users.AsNoTracking().AnyAsync(u => u.Id == userId);

            if (!userExists)
                return null;

            var products = await _context.Products
                .Include(p => p.Marketplace)
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            // Gruparea produselor după ProductId
            var groupedProducts = products
                .GroupBy(x => int.Parse(x.ProductId))
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(product => new ProductDTO
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Quantity = product.Quantity,
                        Marketplaces = product.Marketplace != null
                            ? new List<MarketplaceDTO>
                            {
                                new MarketplaceDTO
                                {
                                    MarketplaceName = product.Marketplace.Name,
                                    Exported = true,
                                    Id = product.Marketplace.Id,
                                    ExportDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
                                }
                            }
                            : new List<MarketplaceDTO>()
                    }).ToList()
                );

            return groupedProducts;
        }

        public async Task<User> RegisterUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(int id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new Exception("Utilizatorul nu a fost găsit.");

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            // Adaugă alte câmpuri care pot fi actualizate

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
