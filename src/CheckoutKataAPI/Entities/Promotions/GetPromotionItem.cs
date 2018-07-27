using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities.Promotions
{
    public class GetPromotionItem
    {
        public int IdProduct { get; set; }

        public decimal QTY{get;set;}

        public decimal PercentDiscount { get; set; }
    }
}
