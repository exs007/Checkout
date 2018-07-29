using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CheckoutKataAPI.Constants;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Exceptions;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.Test.DAL;
using GenFu;
using Xunit;
using CheckoutKataAPI.Entities.Promotions;
using Moq;

namespace CheckoutKataAPI.Test.Services
{
    public class PromotionServiceTest
    {
        private readonly IRepository<BasePromotion> _promotionRepository;
        private readonly IPromotionService _promotionService;

        public PromotionServiceTest()
        {
            _promotionRepository = new FakeRepository<BasePromotion>();
            _promotionService = new PromotionService(_promotionRepository);
            A.Configure<PricePromotion>().
                Fill(p => p.Id, 0).
                Fill(p => p.AssignedProductIds, new List<int>() { 1, 2 });
            A.Configure<BuyXGetYPromotion>().
                Fill(p => p.Id, 0).
                Fill(p => p.ApplyLimit, (int?)null).
                Fill(p => p.BuyItems, new List<BuyPromotionItem>()
                {
                    new BuyPromotionItem()
                    {
                        IdProduct=1,
                        QTY=1,
                    },
                    new BuyPromotionItem()
                    {
                        IdProduct=2,
                        QTY=1.5m,
                    },
                }).Fill(p => p.GetItems, new List<GetPromotionItem>()
                {
                    new GetPromotionItem()
                    {
                        IdProduct=3,
                        QTY=1,
                        PercentDiscount=100
                    },
                    new GetPromotionItem()
                    {
                        IdProduct=4,
                        QTY=1.5m,
                        PercentDiscount =50,
                    },
                });
        }

        [Fact]
        public void CreateNewPricePromotion()
        {
            var item = A.New<PricePromotion>();
            var createdItem = _promotionService.AddPromotion(item);

            var storageItem = _promotionRepository.Select(createdItem.Id);
            Assert.NotNull(storageItem);
            Assert.True(storageItem is PricePromotion);
        }

        [Fact]
        public void CreateNewBuyXGetYPromotion()
        {
            var item = A.New<BuyXGetYPromotion>();
            var createdItem = _promotionService.AddPromotion(item);

            var storageItem = _promotionRepository.Select(createdItem.Id);
            Assert.NotNull(storageItem);
            Assert.True(storageItem is BuyXGetYPromotion);
        }

        [Fact]
        public void CreateNewBuyXGetYPromotionWithoutBuyPartAndThrowException()
        {
            var item = A.New<BuyXGetYPromotion>();
            item.BuyItems = new List<BuyPromotionItem>();
            
            var exception = Assert.ThrowsAny<AppValidationException>(() => _promotionService.AddPromotion(item));
            Assert.Contains(MessageConstants.MISSED_BUY_PART_IN_GET_BUY_PROMOTION, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void CreateNewBuyXGetYPromotionWithoutGetPartAndThrowException()
        {
            var item = A.New<BuyXGetYPromotion>();
            item.GetItems = new List<GetPromotionItem>();
            
            var exception = Assert.ThrowsAny<AppValidationException>(() => _promotionService.AddPromotion(item));
            Assert.Contains(MessageConstants.MISSED_GET_PART_IN_GET_BUY_PROMOTION, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void CreateNotKnownTypeOfPromotionAndThrowException()
        {
            var item = Mock.Of<BasePromotion>();
            
            var exception = Assert.ThrowsAny<ArgumentException>(() => _promotionService.AddPromotion(item));
            Assert.Contains(MessageConstants.PROMOTION_TYPE_IS_UNKNOWN, exception.Message);
        }

        [Fact]
        public void SelectMultiplePricePromotionsFromRepository()
        {
            var item1 = A.New<PricePromotion>();
            var createdItem1 = _promotionRepository.Add(item1);

            var item2 = A.New<PricePromotion>();
            var createdItem2 = _promotionRepository.Add(item2);
            var ids = new List<int>(){ createdItem1.Id, createdItem2.Id};
            
            var existPricePromotions = _promotionService.GetPromotions();

            Assert.Equal(ids.Count, existPricePromotions.Count);
            Assert.True(existPricePromotions.All(p=>p is PricePromotion && ids.Contains(p.Id)));
        }

        [Fact]
        public void SelectZeroPricePromotionsFromRepositoryWithDiffentPromotions()
        {
            var item1 = A.New<BuyXGetYPromotion>();
            var createdItem1 = _promotionRepository.Add(item1);
            
            var existPricePromotions = _promotionService.GetPromotions();
            
            Assert.True(existPricePromotions.All(p=>!(p is PricePromotion)));
        }
    }
}
