namespace ppawproject.Models
{
    public class ProductExportRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public int CurrentMarketplaceId { get; set; }
        public int NewMarketplaceId { get; set; }
        public int UserId { get; set; }
    }
}
