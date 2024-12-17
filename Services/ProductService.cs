using Microsoft.EntityFrameworkCore;

using ppawproject.Database;
using ppawproject.Database.Entities;
using ppawproject.Interfaces;
using ppawproject.Models;

namespace ppawproject.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Product> SaveProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProduct(string productId, int marketplaceId, ProductDTO updatedProduct)
        {
            // Căutăm produsul în funcție de ProductId și MarketplaceId
            var product = await _context.Products
                .Where(p => p.ProductId == productId && p.MarketplaceId == marketplaceId)
                .FirstOrDefaultAsync();

            if (product == null)
                throw new Exception($"Produsul cu id-ul {productId} și marketplaceId {marketplaceId} nu a fost găsit.");

            // Actualizăm câmpurile produsului
            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.Price = updatedProduct.Price;
            product.Quantity = updatedProduct.Quantity;
            product.ExternalCode = updatedProduct.ExternalCode;
            product.ImageURL = updatedProduct.ImageURL;

            // Actualizăm doar UserId și MarketplaceId dacă sunt transmise
            if (updatedProduct.UserId != 0)
                product.UserId = updatedProduct.UserId;

            if (updatedProduct.MarketplaceId != 0)
                product.MarketplaceId = updatedProduct.MarketplaceId;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteProduct(string productId, int marketplaceId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId && p.MarketplaceId == marketplaceId);

            if (product == null)
                throw new Exception("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> AddProductWithMarketplace(ProductExportRequest request)
        {
            // Găsește produsul original pe baza ProductId, MarketplaceId și UserId
            var originalProduct = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.ProductId == request.ProductId &&
                    p.MarketplaceId == request.CurrentMarketplaceId &&
                    p.UserId == request.UserId);

            if (originalProduct == null)
                throw new Exception("Produsul original nu a fost găsit pentru marketplace-ul specificat.");

            // Găsește produsul cu cel mai mare ID
            var lastProduct = await _context.Products
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            // Calculează noul ID
            var newId = (lastProduct?.Id ?? 0) + 1;

            // Creează un nou produs bazat pe cel original
            var newProduct = new Product
            {
                Id = newId, // Folosește ID-ul calculat
                Name = originalProduct.Name,
                Description = originalProduct.Description,
                Price = originalProduct.Price,
                ExternalCode = originalProduct.ExternalCode,
                ProductId = originalProduct.ProductId,
                Quantity = originalProduct.Quantity,
                ImageURL = originalProduct.ImageURL,
                UserId = originalProduct.UserId,
                IsVariant = originalProduct.IsVariant,
                ParentProductId = originalProduct.ParentProductId,
                MarketplaceId = request.NewMarketplaceId, // Setează noul MarketplaceId
            };

            // Adaugă produsul nou în baza de date
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return newProduct;
        }

        public async Task<Product> ExportProductToMarketplace(string productId, int currentMarketplaceId, int newMarketplaceId, int userId)
        {
            var originalProduct = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductId == productId &&
                    p.MarketplaceId == currentMarketplaceId &&
                    p.UserId == userId);

            if (originalProduct == null)
                throw new Exception("Produsul original nu a fost găsit pentru marketplace-ul specificat.");

            var existingProductInNewMarketplace = await _context.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductId == productId &&
                    p.MarketplaceId == newMarketplaceId &&
                    p.UserId == userId);

            if (existingProductInNewMarketplace != null)
                throw new Exception("Produsul există deja pentru acest marketplace.");

            var newProduct = new Product
            {
                ProductId = originalProduct.ProductId,
                Name = originalProduct.Name,
                Description = originalProduct.Description,
                Price = originalProduct.Price,
                ExternalCode = originalProduct.ExternalCode,
                Quantity = originalProduct.Quantity,
                ImageURL = originalProduct.ImageURL,
                UserId = originalProduct.UserId,
                MarketplaceId = newMarketplaceId,
                IsVariant = originalProduct.IsVariant,
                ParentProductId = originalProduct.ParentProductId
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return newProduct;
        }

        // New method to fetch all products for a user
        public async Task<IEnumerable<ProductDTO>> GetAllProductsForUser(int userId)
        {
            var products = await _context.Products
                .Include(p => p.Marketplace)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return products.Select(product => new ProductDTO
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageURL = product.ImageURL,
                Marketplaces = product.Marketplace != null
                    ? new List<MarketplaceDTO>
                    {
                        new MarketplaceDTO
                        {
                            Id = product.Marketplace.Id,
                            Name = product.Marketplace.Name,
                            Exported = true,
                            ExportDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
                        }
                    }
                    : new List<MarketplaceDTO>()
            }).ToList();
        }

        public async Task<IEnumerable<ProductDTO>> GetUserProductsWithMarketplaces(int userId)
        {
            var products = await _context.Products
                .Include(p => p.Marketplace)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            var response = products.GroupBy(p => p.ProductId).Select(group => new ProductDTO
            {
                ProductId = group.Key,
                Name = group.First().Name,
                Description = group.First().Description,
                Price = group.First().Price,
                Quantity = group.First().Quantity,
                ImageURL = group.First().ImageURL,
                Marketplaces = group.Select(product => new MarketplaceDTO
                {
                    Id = product.Marketplace.Id,
                    Name = product.Marketplace.Name,
                    Exported = true,
                    ExportDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
                }).ToList()
            });

            return response;
        }

        public Task<IEnumerable<Product>> GetProductsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> AddNewProduct(AddProductDTO request)
        {
            // Găsește produsul cu cel mai mare ID existent
            var lastProduct = await _context.Products
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            // Calculează noul ID
            var newId = (lastProduct?.Id ?? 0) + 1;

            // Creează un nou produs cu datele din request
            var newProduct = new Product
            {
                Id = newId, // Folosește ID-ul calculat
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ExternalCode = request.ExternalCode,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                ImageURL = request.ImageURL,
                UserId = request.UserId,
                IsVariant = request.IsVariant,
                ParentProductId = request.ParentProductId,
                MarketplaceId = request.MarketplaceId, // Setează MarketplaceId-ul specificat
            };

            // Adaugă produsul nou în baza de date
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return newProduct;
        }
    }
}
