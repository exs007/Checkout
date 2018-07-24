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
            throw new NotImplementedException();
        }

        public BasePromotion AddPromotion(BasePromotion item)
        {
            throw new NotImplementedException();
        }
    }
}
