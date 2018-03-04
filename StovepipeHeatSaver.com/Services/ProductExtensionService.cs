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

        public async Task<Product> GetProductExtensionAsync(Product product)
        {
            var productExtension = await _context.ProductExtensions.SingleAsync(p => p.ProductId == product.Id);
            product.ProductExtension = productExtension;
            return product;
        }

        public async Task<List<Product>> GetProductExtensionsAsync(List<Product> products)
        {
            List<Product> productsWithExtensions = new List<Product>();
            foreach (var product in products)
            {
                productsWithExtensions.Add(await GetProductExtensionAsync(product));
            }
            return productsWithExtensions;
        }
        
        public void DeleteProductExtension(Product product)
        {
            _context.ProductExtensions.Remove(product.ProductExtension);
        }

        public void AddProductExtension(Product product)
        {
            if (product.ProductExtension != null)
            {
                product.ProductExtension.Product = product;
                _context.ProductExtensions.Add(product.ProductExtension);
            }
        }

        public void EditProductExtension(Product product)
        {
            if (product.ProductExtension != null)
            {
                product.ProductExtension.Product = product;
                _context.Entry(product.ProductExtension).State = EntityState.Modified;
            }
        }
    }
}