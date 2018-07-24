using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    throw new AppValidationException(nameof(buyGetPomotion.BuyItems), "Buy part isn't specified");
                }
                if (buyGetPomotion.GetItems?.Count == 0)
                {
                    throw new AppValidationException(nameof(buyGetPomotion.GetItems), "Get part isn't specified");
                }
            }
            else
            {
                throw new ArgumentException("The following promotion type can't be processed");
            }

            return _promotionRepository.Add(item);
        }
    }
}
