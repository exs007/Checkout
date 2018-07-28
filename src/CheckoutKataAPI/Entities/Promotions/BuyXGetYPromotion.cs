using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities.Promotions
{
    public class BuyXGetYPromotion : BasePromotion
    {
        public BuyXGetYPromotion()
        {
            BuyItems = new List<BuyPromotionItem>();
            GetItems = new List<GetPromotionItem>();
        }

        public int? ApplyLimit { get; set; }

        public ICollection<BuyPromotionItem> BuyItems { get; set; }

        public ICollection<GetPromotionItem> GetItems { get; set; }
    }
}
