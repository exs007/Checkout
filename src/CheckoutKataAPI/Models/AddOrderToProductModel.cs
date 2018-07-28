using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Models
{
    public class AddOrderToProductModel
    {
        public string SKU { get; set; }

        public decimal QTY{get;set;}
    }
}
