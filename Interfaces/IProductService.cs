using ppawproject.Database.Entities;
using ppawproject.Models;

namespace ppawproject.Interfaces
{
    public interface IProductService
    {


        Task<Product> SaveProduct(Product product);
        Task<Product> UpdateProduct(string productId, int marketplaceId, ProductDTO updatedProduct);
        Task DeleteProduct(string productId, int marketplaceId);
        Task<Product> ExportProductToMarketplace(string productId, int currentMarketplaceId, int newMarketplaceId, int userId);
        Task<Product> AddProductWithMarketplace(ProductExportRequest request);
        // Add new method for fetching user products
        Task<IEnumerable<Product>> GetProductsByUserId(int userId);
        Task<IEnumerable<ProductDTO>> GetUserProductsWithMarketplaces(int userId);
        Task<IEnumerable<ProductDTO>> GetAllProductsForUser(int userId);

        Task<Product> AddNewProduct(AddProductDTO request);
    }
}
