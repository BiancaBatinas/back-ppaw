using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ppawproject.Database;
using ppawproject.Database.Entities;
using ppawproject.Interfaces;
using ppawproject.Models;

namespace ppawproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IProductService _productService;

        public ProductController(DatabaseContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        // GET: api/product/{userId}
        [HttpGet("product/{userId}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetUserProductsWithMarketplaces(int userId)
        {
            try
            {
                var products = await _productService.GetUserProductsWithMarketplaces(userId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // POST: api/product/saveProduct
        [HttpPost("saveProduct")]
        public async Task<IActionResult> SaveProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var savedProduct = await _productService.SaveProduct(product);
            return Ok(savedProduct);
        }

        [HttpPut("updateProductMarket/{productId}/{marketplaceId}")]
        public async Task<IActionResult> UpdateProduct(string productId, int marketplaceId, [FromBody] ProductDTO updatedProduct)
        {
            try
            {
                // Apelăm serviciul pentru a actualiza produsul, folosind productId și marketplaceId
                var updated = await _productService.UpdateProduct(productId, marketplaceId, updatedProduct);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


        [HttpDelete("deleteProduct/{productId}/{marketplaceId}")]
        public async Task<IActionResult> DeleteProduct(string productId, int marketplaceId)
        {
            try
            {
                await _productService.DeleteProduct(productId, marketplaceId);
                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting product: {ex.Message}");
            }
        }

        [HttpPost("addProductWithMarketplace")]
        public async Task<IActionResult> AddProductWithMarketplace(
            [FromBody] ProductExportRequest request)
        {
            try 
            {
                var newProduct = await _productService.AddProductWithMarketplace(request);
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        


        [HttpPost("exportProductToMarketplace")]
        public async Task<IActionResult> ExportProductToMarketplace(
            [FromBody] ProductExportRequest request)
        {
            try 
            {
                var newProduct = await _productService.ExportProductToMarketplace(
                    request.ProductId, 
                    request.CurrentMarketplaceId, 
                    request.NewMarketplaceId, 
                    request.UserId
                );
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addNewProduct")]
        public async Task<IActionResult> AddNewProduct([FromBody] AddProductDTO request)
        {
            try
            {
                var newProduct = await _productService.AddNewProduct(request);
                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
