namespace ppawproject.Database.Entities
{
    public class Subscription
    {
        public int Id { get; set; } = default;

        public string Name { get; set; } = string.Empty;

        public int MaxMarketplaces { get; set; } = default;

        public int MaxProducts { get; set; } = default;

        public decimal Price { get; set; } = default;

        public virtual List<User> Users { get; set; } = new List<User>();
    }
}
