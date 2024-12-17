using System.ComponentModel.DataAnnotations.Schema;

namespace ppawproject.Database.Entities
{
    public class Marketplace
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public byte[] Image { get; set; }

        public int PriceModifier { get; set; }

        public virtual List<Product> Products { get; set; } = new List<Product>();

        public int UserId { get; set; }

        public virtual User User { get; set; }

        [NotMapped] // Acest câmp nu va fi salvat în baza de date
        public IFormFile ImageFile { get; set; }
    }
}
