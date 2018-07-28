using CheckoutKataAPI.Workflow.Base;
using CheckoutKataAPI.Workflow.Orders.Actions;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Entities.Products;
using Xunit;
using CheckoutKataAPI.Entities.Promotions;

namespace CheckoutKataAPI.Test.Workflow.Actions
{
    public class PricePromotionsActionTest
    {
        private readonly IWorkflowProcessorContext _executingContext;
        private readonly PricePromotionsAction _action;
        private readonly OrderCalculationContext _context;
        private readonly decimal _firstOrderToProductPrice = 10.5m;
        private readonly decimal _secondOrderToProductPrice = 11m;
        private readonly decimal _promoOrderToProductPrice = 10m;

        public PricePromotionsActionTest()
        {
            _executingContext = Mock.Of<IWorkflowProcessorContext>();
            _action = new PricePromotionsAction();
            _context = new OrderCalculationContext(new Order()
            {
                Id=1,
            });
            _context.OrderToProducts = new List<OrderToProduct>()
            {
                new  OrderToProduct()
                {
                    IdProduct = 1,
                    QTY =1,
                    Price = _firstOrderToProductPrice,
                },
                new OrderToProduct()
                {
                    IdProduct = 2,
                    QTY =1.5m,
                    Price = _secondOrderToProductPrice,
                },
                new OrderToProduct()
                {
                    IdProduct = 11,
                    Price = _promoOrderToProductPrice,
                    IdUsedBuyGetPromotion = 1
                }
            };
        }

        [Fact]
        public void ApplyOnePricePromotionToTwoProductsAndCheckPrices()
        {
            _context.ActivePromotions = new List<BasePromotion>()
            {
                new PricePromotion()
                {
                    Id=2,
                    PriceDiscount = 2.5m,
                    AssignedProductIds = new List<int>()
                    {
                        1, 2
                    }
                }
            };
            _action.ExecuteAction(_context, _executingContext);

            Assert.True(_context.OrderToProducts.First(p => p.IdProduct == 1).
                            Price == _firstOrderToProductPrice - 2.5m);
            Assert.True(_context.OrderToProducts.First(p => p.IdProduct == 2).
                            Price == _secondOrderToProductPrice - 2.5m);
        }

        [Fact]
        public void ApplyTwoPricePromotionsToOneProductsAndCheckThatTheBiggestDiscountWasApplied()
        {
            _context.ActivePromotions = new List<BasePromotion>()
            {
                new PricePromotion()
                {
                    Id=2,
                    PriceDiscount = 2.5m,
                    AssignedProductIds = new List<int>()
                    {
                        1
                    }
                },
                new PricePromotion()
                {
                    Id=3,
                    PriceDiscount = 5m,
                    AssignedProductIds = new List<int>()
                    {
                        1
                    }
                }
            };
            _action.ExecuteAction(_context, _executingContext);

            Assert.True(_context.OrderToProducts.First(p => p.IdProduct == 1).
                            Price == _firstOrderToProductPrice - 5m);
        }

        [Fact]
        public void ApplyOnePricePromotionWithDiscountBiggerThanPriceToOneProductAndCheckThahResultPriceIsNotNegative()
        {
            _context.ActivePromotions = new List<BasePromotion>()
            {
                new PricePromotion()
                {
                    Id=2,
                    PriceDiscount = 1000m,
                    AssignedProductIds = new List<int>()
                    {
                        1
                    }
                },
            };
            _action.ExecuteAction(_context, _executingContext);

            Assert.True(_context.OrderToProducts.First(p => p.IdProduct == 1).
                            Price >= 0);
        }

        [Fact]
        public void TryToApplyOnePricePromotionToPromotionProductAndCheckThatPricePromotionWasNotApplied()
        {
            _context.ActivePromotions = new List<BasePromotion>()
            {
                new PricePromotion()
                {
                    Id=2,
                    PriceDiscount = 2.5m,
                    AssignedProductIds = new List<int>()
                    {
                        11
                    }
                },
            };
            _action.ExecuteAction(_context, _executingContext);

            Assert.True(_context.OrderToProducts.First(p => p.IdProduct == 11).
                            Price == _promoOrderToProductPrice);
        }

        [Fact]
        public void TryToApplyOnePricePromotionWhichIsNotSutiableForAnyExistProductsAndCheckThatPricesWereNotChanged()
        {
            _context.ActivePromotions = new List<BasePromotion>()
            {
                new PricePromotion()
                {
                    Id=2,
                    PriceDiscount = 2.5m,
                    AssignedProductIds = new List<int>()
                    {
                        100
                    }
                },
            };
            _action.ExecuteAction(_context, _executingContext);

            Assert.True(_context.OrderToProducts.First(p => p.IdProduct == 1).
                            Price == _firstOrderToProductPrice);
            Assert.True(_context.OrderToProducts.First(p => p.IdProduct == 2).
                            Price == _secondOrderToProductPrice);
        }
    }
}
