using Cstieg.Sales.Models;
using Cstieg.Sales.Interfaces;
using StovepipeHeatSaver.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace StovepipeHeatSaver.Services
{
    public class ProductExtensionService : IProductExtensionService
    {
        private readonly IProductExtensionContext _context;

        public ProductExtensionService(IProductExtensionContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductExtension(Product product)
        {
            var productExtension = await _context.ProductExtensions.SingleAsync(p => p.ProductId == product.Id);
            product.ProductExtension = productExtension;
            return product;
        }

        public async Task<List<Product>> GetProductExtensions(List<Product> products)
        {
            List<Product> productsWithExtensions = new List<Product>();
            foreach (var product in products)
            {
                productsWithExtensions.Add(await GetProductExtension(product));
            }
            return productsWithExtensions;
        }
        
    }
}