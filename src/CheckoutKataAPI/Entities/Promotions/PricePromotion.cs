using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities.Promotions
{
    public class PricePromotion : BasePromotion
    {
        public PricePromotion()
        {
            AssignedProductIds = new List<int>();
        }

        public decimal PriceDiscount { get; set; }

        public ICollection<int> AssignedProductIds { get; set; }
    }
}
