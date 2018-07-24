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
            var toReturn = new List<Product>();

            foreach (var id in ids)
            {
                var product = _productRepository.Select(id);
                if (product == null)
                {
                    throw new AppValidationException("Invalid product id");
                }

                toReturn.Add(product);
            }

            return toReturn;
        }

        public Product GetProduct(string sku)
        {
            var items = _productRepository.Select(p => p.SKU == sku);
            if (items.Count > 1)
            {
                throw new Exception("Multiple products with the same SKU");
            }

            return items.FirstOrDefault();
        }

        public Product AddProduct(Product item)
        {
            if (!Enum.IsDefined(typeof(PriceType), item.PriceType))
            {
                throw new AppValidationException(nameof(item.PriceType), "Invalid price type");
            }

            var duplicatesExist = _productRepository.Select(p => p.SKU == item.SKU).Any();
            if (duplicatesExist)
            {
                throw new AppValidationException(nameof(item.SKU), "Exist SKU");
            }

            return _productRepository.Add(item);
        }
    }
}
