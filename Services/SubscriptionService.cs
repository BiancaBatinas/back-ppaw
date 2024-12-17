using Microsoft.EntityFrameworkCore;

using ppawproject.Database;
using ppawproject.Database.Entities;
using ppawproject.Interfaces;

namespace ppawproject.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly DatabaseContext _context;

        public SubscriptionService(DatabaseContext context)
        {
            _context = context;
        }

        // Obține abonamentele predefinite
        public List<Subscription> GetPredefinedSubscriptions()
        {
            return new List<Subscription>
            {
                new Subscription { Id = 1, Name = "Free", MaxMarketplaces = 2, MaxProducts = 50, Price = 0 },
                new Subscription { Id = 2, Name = "Pro", MaxMarketplaces = 3, MaxProducts = 500, Price = 29 },
                new Subscription { Id = 3, Name = "Business", MaxMarketplaces = int.MaxValue, MaxProducts = int.MaxValue, Price = 99 }
            };
        }

        // Atribuie un abonament unui utilizator
        public bool AssignSubscriptionToUser(int userId, int subscriptionId)
        {
            var user = _context.Users.Find(userId);
            if (user == null) return false;

            var subscription = _context.Subscriptions.Find(subscriptionId);
            if (subscription == null) return false;

            user.SubscriptionId = subscriptionId;

            _context.SaveChanges();
            return true;
        }

        // Verifică dacă utilizatorul poate adăuga un marketplace
        public bool CanAddMarketplace(int userId)
        {
            var user = _context.Users
                .Include(u => u.Subscription)
                .Include(u => u.Marketplaces)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null) return false;

            return user.Marketplaces.Count < user.Subscription.MaxMarketplaces;
        }

        // Verifică dacă utilizatorul poate adăuga un produs
        public bool CanAddProduct(int userId)
        {
            var user = _context.Users
                .Include(u => u.Subscription)
                .Include(u => u.Products)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null) return false;

            return user.Products.Count < user.Subscription.MaxProducts;
        }

        // Adaugă un marketplace utilizatorului
        public bool AddMarketplaceToUser(int userId, Marketplace marketplace)
        {
            var user = _context.Users
                .Include(u => u.Subscription)
                .Include(u => u.Marketplaces)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null || !CanAddMarketplace(userId)) return false;

            user.Marketplaces.Add(marketplace);
            _context.SaveChanges();
            return true;
        }

    }
}
