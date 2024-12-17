using ppawproject.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ppawproject.Models
{
    public class MarketplaceDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public byte[] Image { get; set; }

        public int PriceModifier { get; set; }

        public int UserId { get; set; }

        public string MarketplaceName { get; set; }

        public bool Exported { get; set; }

        public string ExportDate { get; set; }
    }
}
