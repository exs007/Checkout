using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Constants;
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

        public ICollection<Product> GetProducts(ICollection<int> ids, bool ignoreNotFoundProducts=false)
        {
            var toReturn = new List<Product>();

            foreach (var id in ids)
            {
                var product = _productRepository.Select(id);
                if (product == null)
                {
                    if (!ignoreNotFoundProducts)
                    {
                        throw new AppValidationException(MessageConstants.NOT_FOUND_PRODUCT_ID);
                    }
                }
                else
                {
                    toReturn.Add(product);
                }
            }

            return toReturn;
        }

        public Product GetProduct(string sku)
        {
            var items = _productRepository.Select(p =>StringComparer.InvariantCultureIgnoreCase.Equals(p.SKU, sku));
            if (items.Count > 1)
            {
                throw new Exception(MessageConstants.INVALID_SETUP_MULTIPLE_PRODUCTS_WITH_SAME_SKU);
            }

            return items.FirstOrDefault();
        }

        public Product AddProduct(Product item)
        {
            if (!Enum.IsDefined(typeof(PriceType), item.PriceType))
            {
                throw new AppValidationException(nameof(item.PriceType), MessageConstants.NOT_VALID_PRICE_TYPE_IN_PRODUCT);
            }

            var duplicatesExist = _productRepository.Select(p => p.SKU == item.SKU).Any();
            if (duplicatesExist)
            {
                throw new AppValidationException(nameof(item.SKU), MessageConstants.PRODUCT_SKU_DUPLICATE);
            }

            return _productRepository.Add(item);
        }
    }
}
