using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities.Orders
{
    public class OrderToProduct
    {
        public int IdProduct { get; set; }

        public decimal QTY{get;set;}

        public decimal Price{get;set;}

        public decimal Amount => QTY * Price;

        public int? IdUsedBuyGetPromotion{get;set;}
    }
}
