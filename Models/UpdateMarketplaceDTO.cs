namespace ppawproject.Models
{
    public class UpdateMarketplaceDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public byte[] Image { get; set; }

        public int PriceModifier { get; set; }

        public int UserId { get; set; }
    }
}
