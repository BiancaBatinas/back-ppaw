using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ppawproject.Database.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string ExternalCode { get; set; } = string.Empty;

    public string ProductId { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string ImageURL { get; set; } = string.Empty;

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }

    public bool IsVariant { get; set; }

    public int? ParentProductId { get; set; }

    [ForeignKey(nameof(ParentProductId))]
    public virtual Product ParentProduct { get; set; }

    public virtual List<Product> Variants { get; set; } = new List<Product>();

    public int MarketplaceId { get; set; }

    [ForeignKey(nameof(MarketplaceId))]
    public virtual Marketplace Marketplace { get; set; }
}
