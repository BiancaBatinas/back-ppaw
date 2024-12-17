using ppawproject.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ppawproject.Models
{
    public class AddProductDTO
    {
       
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string ExternalCode { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string ImageURL { get; set; } = string.Empty;

        public int UserId { get; set; }

        public bool IsVariant { get; set; }

        public int? ParentProductId { get; set; }

        public int MarketplaceId { get; set; }

      
    }
}
