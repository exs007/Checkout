using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Products;
using CheckoutKataAPI.Exceptions;

namespace CheckoutKataAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public ICollection<Product> GetProducts(ICollection<int> ids)
        {
            throw new NotImplementedException();
        }

        public Product GetProduct(string sku)
        {
            throw new NotImplementedException();
        }

        public Product AddProduct(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
