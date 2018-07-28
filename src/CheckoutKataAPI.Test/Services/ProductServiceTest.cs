using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Exceptions;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.Test.DAL;
using GenFu;
using Xunit;
using CheckoutKataAPI.Entities.Products;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
namespace CheckoutKataAPI.Test.Services
{
    public class ProductServiceTest
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IProductService _productService;
        private readonly string _baseSKUCode="SKU";
        private readonly int _baseSKUCodeIncrement = 1;

        public ProductServiceTest()
        {
            _productRepository = new FakeRepository<Product>();
            _productService = new ProductService(_productRepository);
            A.Configure<Product>().
                Fill(p => p.Id, 0).
                Fill(p=>p.SKU, _baseSKUCode + ++_baseSKUCodeIncrement);
        }

        [Fact]
        public void CreateNewProductWithUniqueSKU()
        {
            var product = A.New<Product>();
            product = _productService.AddProduct(product);

            var storageProduct = _productRepository.Select(product.Id);
            Assert.NotNull(storageProduct);
        }

        [Fact]
        public void CreateNewProductWithExistSKUAndThrowException()
        {
            var product1 = A.New<Product>();
            product1 = _productService.AddProduct(product1);
            var product2 = A.New<Product>();
            product2.SKU = product2.SKU;

            var exception = Assert.ThrowsAny<AppValidationException>(() => _productService.AddProduct(product2));
            Assert.Contains("Exist SKU", exception.Messages.Select(p=>p.Message));
        }

        [Fact]
        public void CreateNewProductWithInvalidPriceTypeAndThrowException()
        {
            var product = A.New<Product>();
            product.PriceType = 0;

            var exception = Assert.ThrowsAny<AppValidationException>(() => _productService.AddProduct(product));
            Assert.Contains("Invalid price type", exception.Messages.Select(p=>p.Message));
        }

        [Fact]
        public void SelectMultipleExistProductsByIdsFromRepository()
        {
            var product1 = A.New<Product>();
            product1 = _productRepository.Add(product1);
            var product2 = A.New<Product>();
            product2 =_productRepository.Add(product2);

            var ids = new List<int>(){ product1.Id, product2.Id };
            var products = _productService.GetProducts(ids);
            Assert.Equal(ids.Count, products.Count);
            Assert.All(products, p => ids.Contains(p.Id));
        }

        [Fact]
        public void SelectNotExistProductByIdAndThrowException()
        {
            var id = 1000000;
            var ids = new List<int>(){ id };

            var exception = Assert.Throws<AppValidationException>(() => _productService.GetProducts(ids));
            Assert.Contains("Invalid product id", exception.Messages.Select(p=>p.Message));
        }

        [Fact]
        public void SelectNotExistProductByIdWithIgnoreNotFoundParamAndGetEmptyCollection()
        {
            var id = 1000000;
            var ids = new List<int>(){ id };

            var products = _productService.GetProducts(ids, true);
            Assert.Equal(0, products.Count);
        }

        [Fact]
        public void SelectExistProductBySKUFromRepository()
        {
            var product = A.New<Product>();
            product = _productRepository.Add(product);

            var existProduct = _productService.GetProduct(product.SKU);
            Assert.NotNull(existProduct);
        }

        [Fact]
        public void SelectNotExistProductBySKUAndReturnNullFromRepository()
        {
            var notExistSku = _baseSKUCode + 1000000;

            var product = _productService.GetProduct(notExistSku);
            Assert.Null(product);
        }

        [Fact]
        public void SelectMoreMultipleProductsBySKUFromRepository()
        {
            var product1 = A.New<Product>();
            product1 = _productRepository.Add(product1);
            var product2 = A.New<Product>();
            product2.SKU = product1.SKU;
            product2 =_productRepository.Add(product2);
            
            var exception = Assert.Throws<Exception>(() => _productService.GetProduct(product1.SKU));
            Assert.Contains("Multiple products with the same SKU", exception.Message);
        }
    }
}
