using Microsoft.AspNetCore.Mvc;

using ppawproject.Database.Entities;
using ppawproject.Interfaces;
using ppawproject.Models;

namespace ppawproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketplaceController : ControllerBase
    {
        private readonly IMarketplaceService _marketplaceService;

        public MarketplaceController(IMarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        // GET: api/marketplace/{userId}
        [HttpGet("marketplace/{userId}")]
        public async Task<IActionResult> GetMarketplacesByUser(int userId)
        {
            var marketplaces = await _marketplaceService.GetMarketplacesByUser(userId);

            if (!marketplaces.Any())
            {
                return NotFound("Nu s-au găsit marketplace-uri pentru acest utilizator.");
            }

            return Ok(marketplaces);
        }

        // POST: api/marketplace/add
        [HttpPost("add")]
        public async Task<IActionResult> AddMarketplace([FromForm] AddMarketplaceDTO marketplace)
        {

            var da = new byte[1];
            // Convertim imaginea din IFormFile în byte[]
            if (marketplace.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await marketplace.ImageFile.CopyToAsync(memoryStream);
                   da = memoryStream.ToArray();
                }
            }

            // Creăm un obiect Marketplace
            var marketplace_ = new Marketplace
            {
                Name = marketplace.Name,
                Link = marketplace.Link,
                Image = da,
                PriceModifier = marketplace.PriceModifier,
                UserId = marketplace.UserId
            };

            var addedMarketplace = await _marketplaceService.AddMarketplace(marketplace_);

            return CreatedAtAction(nameof(GetMarketplacesByUser), new { userId = addedMarketplace.UserId }, addedMarketplace);
        }

        // PUT: api/marketplace/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateMarketplace([FromQuery] int id, [FromBody] UpdateMarketplaceDTO updatedMarketplace)
        {
            try 
            {
                var marketplace = await _marketplaceService.UpdateMarketplace(id, updatedMarketplace);
                return Ok(marketplace);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/marketplace/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMarketplace(int id)
        {
            var result = await _marketplaceService.DeleteMarketplace(id);
            
            if (result)
                return Ok("Marketplace-ul a fost șters cu succes.");
            
            return NotFound("Marketplace-ul nu a fost găsit.");
        }
    }
}
