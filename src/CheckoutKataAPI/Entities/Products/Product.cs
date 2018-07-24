using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities.Products
{
    public class Product : BaseEntity
    {
        public string SKU { get; set; }

        public PriceType PriceType { get; set; }

        public decimal Price { get;set;}
    }
}
