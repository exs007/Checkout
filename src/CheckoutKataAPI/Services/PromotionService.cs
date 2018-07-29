using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Constants;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Promotions;
using CheckoutKataAPI.Exceptions;

namespace CheckoutKataAPI.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IRepository<BasePromotion> _promotionRepository;

        public PromotionService(IRepository<BasePromotion> promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public ICollection<BasePromotion> GetPromotions()
        {
            return _promotionRepository.SelectAll();
        }

        public BasePromotion AddPromotion(BasePromotion item)
        {
            if (item is PricePromotion)
            {
                var pricePromotion = item as PricePromotion;
            } 
            else if (item is BuyXGetYPromotion)
            {
                var buyGetPomotion = item as BuyXGetYPromotion;
                if (buyGetPomotion.BuyItems?.Count == 0)
                {
                    throw new AppValidationException(nameof(buyGetPomotion.BuyItems), MessageConstants.MISSED_BUY_PART_IN_GET_BUY_PROMOTION);
                }
                if (buyGetPomotion.GetItems?.Count == 0)
                {
                    throw new AppValidationException(nameof(buyGetPomotion.GetItems), MessageConstants.MISSED_GET_PART_IN_GET_BUY_PROMOTION);
                }
            }
            else
            {
                throw new ArgumentException(MessageConstants.PROMOTION_TYPE_IS_UNKNOWN);
            }

            return _promotionRepository.Add(item);
        }
    }
}
