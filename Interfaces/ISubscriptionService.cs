using ppawproject.Database.Entities;

namespace ppawproject.Interfaces
{
    public interface ISubscriptionService
    {
        public List<Subscription> GetPredefinedSubscriptions();
        public bool CanAddMarketplace(int userId);
    }
}
