using ppawproject.Database.Entities;
using System.Collections.Generic;

namespace ppawproject.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ExternalCode { get; set; }

        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public string ImageURL { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsVariant { get; set; }
        public int? ParentProductId { get; set; }
        public int MarketplaceId { get; set; }

        public List<MarketplaceDTO> Marketplaces { get; set; } = new List<MarketplaceDTO>();
    }
}
