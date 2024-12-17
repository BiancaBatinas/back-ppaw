namespace ppawproject.Database.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int SubscriptionId { get; set; }

        public virtual Subscription Subscription { get; set; } = new Subscription();

        public virtual List<Product> Products { get; set; } = new List<Product>();

        public virtual List<Marketplace> Marketplaces { get; set; } = new List<Marketplace>();
    }
}
