using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Products;

namespace CheckoutKataAPI.Services
{
    public interface IProductService
    {
        ICollection<Product> GetProducts(ICollection<int> ids, bool ignoreNotFoundProducts=false);

        Product GetProduct(string sku);

        Product AddProduct(Product item);
    }
}
