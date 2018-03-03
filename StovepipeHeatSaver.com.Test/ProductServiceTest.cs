using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Cstieg.Sales;
using Cstieg.Sales.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StovepipeHeatSaver.Models;
using StovepipeHeatSaver.Services;
using System.Data.Entity;
using StovepipeHeatSaver.com.Test.Repositories;

namespace StovepipeHeatSaver.com.Test
{
    [TestClass]
    public class ProductServiceTest
    {
        StovepipeHeatSaverTestContext context = new StovepipeHeatSaverTestContext();
        private TransactionScope _transactionScope;
        private ProductService _productService;
        private ProductExtensionService _productExtensionService;

        [TestInitialize]
        public virtual void Initialize()
        {
            _productExtensionService = new ProductExtensionService(context);
            _productService = new ProductService(context, _productExtensionService);
            _transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var fakeProducts = new List<Product>()
            {
                new Product() { Name = "Product 1", DisplayOnFrontPage = true, DoNotDisplay = false },
                new Product() { Name = "Product 2", DisplayOnFrontPage = true, DoNotDisplay = false },
                new Product() { Name = "Product 3", DisplayOnFrontPage = false, DoNotDisplay = false },
                new Product() { Name = "Product 4", DisplayOnFrontPage = false, DoNotDisplay = false },
                new Product() { Name = "Product 5", DisplayOnFrontPage = false, DoNotDisplay = true }
            };

            context.Products.AddRange(fakeProducts);
            context.SaveChanges();

            var fakeProductExtensions = new List<ProductExtension>()
            {
                new ProductExtension() { Diameter = 6, ProductId = fakeProducts[0].Id },
                new ProductExtension() { Diameter = 6, ProductId = fakeProducts[1].Id },
                new ProductExtension() { Diameter = 9, ProductId = fakeProducts[2].Id },
                new ProductExtension() { Diameter = 12, ProductId = fakeProducts[3].Id },
                new ProductExtension() { Diameter = 15, ProductId = fakeProducts[4].Id }
            };
            context.ProductExtensions.AddRange(fakeProductExtensions);
            context.SaveChanges();
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            Transaction.Current.Rollback();
            _transactionScope.Dispose();
        }

        [TestMethod]
        public async Task GetProductExtensionAsync()
        {
            // Arrange
            var product1 = await context.Products.FirstAsync(p => p.Name == "Product 1");

            // Act
            var product = await _productExtensionService.GetProductExtension(product1);

            // Assert
            Assert.AreEqual(6, product.ProductExtension.Diameter);
        }

        [TestMethod]
        public async Task GetProductExtensionsAsync()
        {
            // Arrange
            var products = await context.Products.ToListAsync();

            // Act
            products = await _productExtensionService.GetProductExtensions(products);

            // Assert
            Assert.IsNotNull(products[0].ProductExtension.Diameter);
            Assert.AreEqual(5, products.Count);
        }

        [TestMethod]
        public async Task GetProductsForCircumferenceAsync()
        {
            // Act
            var products = await _productService.GetProductsForCircumferenceAsync(18.84M);

            // Assert
            Assert.AreEqual(2, products.Count);
            Assert.IsTrue(products.Exists(p => p.Name == "Product 1"));
            Assert.IsTrue(products.Exists(p => p.Name == "Product 2"));
            Assert.IsTrue(products[0].ProductExtension.Diameter == 6);
        }
    }
}
