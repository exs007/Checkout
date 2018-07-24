using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities.Promotions
{
    public class BuyPromotionItem
    {
        public int IdProduct { get; set; }

        public decimal Amount{get;set;}
    }
}
