using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Promotions;

namespace CheckoutKataAPI.Services
{
    public interface IPromotionService
    {
        ICollection<BasePromotion> GetPromotions();

        BasePromotion AddPromotion(BasePromotion item);
    }
}
