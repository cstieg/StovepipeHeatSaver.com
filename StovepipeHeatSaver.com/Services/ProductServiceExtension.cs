using Cstieg.Sales;
using Cstieg.Sales.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StovepipeHeatSaver.Services
{
    public static class ProductServiceExtension
    {
        public static async Task<List<Product>> GetProductsForCircumferenceAsync(this ProductService productService, decimal circumference)
        {
            var products = await productService.GetDisplayProductsAsync();
            return products.FindAll(p => circumference >= p.ProductExtension.MinCircumference
                                          && circumference <= p.ProductExtension.MaxCircumference);
        }

    }
}