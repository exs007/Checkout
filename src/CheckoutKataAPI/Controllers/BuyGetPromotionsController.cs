using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Promotions;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutKataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricePromotionsController : BaseApiController 
    {
        private readonly IPromotionService _promotionService;

        public PricePromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost]
        public Result<BasePromotion> Post([FromBody]BuyXGetYPromotion item)
        {
            return _promotionService.AddPromotion(item);
        }
    }
}
