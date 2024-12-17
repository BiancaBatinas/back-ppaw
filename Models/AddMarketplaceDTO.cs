using ppawproject.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ppawproject.Models
{
    public class AddMarketplaceDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }



        public int PriceModifier { get; set; }

   
        public int UserId { get; set; }


        [NotMapped] 
        public IFormFile ImageFile { get; set; }
    }
}
