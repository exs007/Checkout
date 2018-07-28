using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Exceptions;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.Test.DAL;
using GenFu;
using Xunit;
using CheckoutKataAPI.Entities.Products;
using CheckoutKataAPI.Workflow.Base;
using CheckoutKataAPI.Test.Workflow;
using Moq;
using CheckoutKataAPI.Workflow.Orders.Actions;
using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Entities.Promotions;

namespace CheckoutKataAPI.Test.Workflow.Actions
{
    public class BuyXGetYPromotionsActionTest
    {
        private readonly IWorkflowProcessorContext _executingContext;
        private readonly BuyXGetYPromotionsAction _action;
        private readonly Product _firstGetPromoProduct = new Product()
        {
            Id = 11,
            Price = 10.5m,
            PriceType = PriceType.PerQuantity
        };
        private readonly Product _secondGetPromoProduct = new Product()
        {
            Id = 12,
            Price = 10.5m,
            PriceType = PriceType.PerQuantity
        };

        public BuyXGetYPromotionsActionTest()
        {       
            var executingContextSetup = new Mock<IWorkflowProcessorContext>();
            var productServiceSetup = new Mock<IProductService>();
            productServiceSetup.Setup(p => p.GetProducts(new List<int>() {11, 12}, true)).Returns(new List<Product>()
            {
                _firstGetPromoProduct,
                _secondGetPromoProduct
            });

            executingContextSetup.Setup(p=>p.Resolve<IProductService>()).
                Returns(productServiceSetup.Object);
            _executingContext = executingContextSetup.Object;
            _action = new BuyXGetYPromotionsAction();
        }

        private OrderCalculationContext GetContextForBuyTwoGetTwoOneRule(int? limit=null)
        {
            var context = new OrderCalculationContext(new Order(){ Id=1 });
            context.OrderToProducts = new List<OrderToProduct>()
            {
                new OrderToProduct()
                {
                    IdProduct = 1,
                    QTY = 4,
                },
                new OrderToProduct()
                {
                    IdProduct = 2,
                    QTY = 2,
                }
            };
            context.ActivePromotions = new List<BasePromotion>()
            {
                new BuyXGetYPromotion()
                {
                    Id=1,
                    ApplyLimit = limit,
                    BuyItems = new List<BuyPromotionItem>()
                    {
                        new BuyPromotionItem()
                        {
                            IdProduct=1,
                            QTY=2,
                        },
                        new BuyPromotionItem()
                        {
                            IdProduct=2,
                            QTY=1,
                        }
                    },
                    GetItems = new List<GetPromotionItem>()
                    {
                        new GetPromotionItem()
                        {
                            IdProduct=11,
                            QTY=2,
                            PercentDiscount=100,
                        },
                        new GetPromotionItem()
                        {
                            IdProduct=12,
                            QTY=1,
                            PercentDiscount=50,
                        }
                    }
                }
            };

            return context;
        }

        [Fact]
        public void GetTwoPromoProductRecordsOnOneRuleAndValidateQTYs()
        {
            var context = GetContextForBuyTwoGetTwoOneRule();
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(4, context.OrderToProducts.Count);
            var firstPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 11);
            Assert.True(firstPromotionProduct?.QTY ==4);
            var secondPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 12);
            Assert.True(secondPromotionProduct?.QTY ==2);
        }

        [Fact]
        public void GetTwoPromoProductRecordsOnOneRuleAndValidatePrices()
        {
            var context = GetContextForBuyTwoGetTwoOneRule();
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(4, context.OrderToProducts.Count);
            var firstPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 11);
            Assert.True(firstPromotionProduct?.Price ==0);
            var secondPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 12);
            Assert.True(secondPromotionProduct?.Price == _secondGetPromoProduct.Price * 0.5m);
        }

        [Fact]
        public void GetTwoPromoProductRecordsOnOneRuleAndChechMarkedAsPromotionProducts()
        {
            var context = GetContextForBuyTwoGetTwoOneRule();
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(4, context.OrderToProducts.Count);
            var firstPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 11);
            Assert.True(firstPromotionProduct?.IdUsedBuyGetPromotion ==1);
            var secondPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 12);
            Assert.True(secondPromotionProduct?.IdUsedBuyGetPromotion ==1);
        }

        [Fact]
        public void GetTwoPromoProductRecordsOnOneRuleWithOneTimeLimitAndValidateQTYs()
        {
            var context = GetContextForBuyTwoGetTwoOneRule(1);
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(4, context.OrderToProducts.Count);
            var firstPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 11);
            Assert.True(firstPromotionProduct?.QTY ==2);
            var secondPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 12);
            Assert.True(secondPromotionProduct?.QTY ==1);
        }

        private OrderCalculationContext GetContextWithMultipleRulesForTheSameProduct()
        {
            var context = new OrderCalculationContext(new Order(){ Id=1 });
            context.OrderToProducts = new List<OrderToProduct>()
            {
                new OrderToProduct()
                {
                    IdProduct = 1,
                    QTY = 4,
                }
            };
            context.ActivePromotions = new List<BasePromotion>()
            {
                new BuyXGetYPromotion()
                {
                    Id=1,
                    BuyItems = new List<BuyPromotionItem>()
                    {
                        new BuyPromotionItem()
                        {
                            IdProduct=1,
                            QTY=2,
                        },
                    },
                    GetItems = new List<GetPromotionItem>()
                    {
                        new GetPromotionItem()
                        {
                            IdProduct=11,
                            QTY=2,
                            PercentDiscount=100,
                        },
                    }
                },
                new BuyXGetYPromotion()
                {
                    Id=2,
                    BuyItems = new List<BuyPromotionItem>()
                    {
                        new BuyPromotionItem()
                        {
                            IdProduct=1,
                            QTY=1,
                        }
                    },
                    GetItems = new List<GetPromotionItem>()
                    {
                        new GetPromotionItem()
                        {
                            IdProduct=12,
                            QTY=1.1m,
                            PercentDiscount=50,
                        }
                    }
                }
            };

            return context;
        }

        [Fact]
        public void GetPromoProductRecordPerRuleForTwoRulesAndCheckQTYs()
        {
            var context = GetContextWithMultipleRulesForTheSameProduct();
            _action.ExecuteAction(context, _executingContext);
            
            Assert.Equal(3, context.OrderToProducts.Count);
            var firstPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 11);
            Assert.True(firstPromotionProduct?.QTY ==4);
            var secondPromotionProduct = context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 12);
            Assert.True(secondPromotionProduct?.QTY ==4.4m);
        }

        [Fact]
        public void GetNoPromoProductsByRules()
        {
            var context = GetContextWithMultipleRulesForTheSameProduct();
            context.ActivePromotions = new List<BasePromotion>();
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(1, context.OrderToProducts.Count);
            Assert.True(context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 1)!=null);
        }

        [Fact]
        public void TryToApplyBuyGetRuleWithNotExistInStoreProductFromGetSectionAndCheckThatItWasNotAdded()
        {
            var context = new OrderCalculationContext(new Order(){ Id=1 });
            context.OrderToProducts = new List<OrderToProduct>()
            {
                new OrderToProduct()
                {
                    IdProduct = 1,
                    QTY = 4,
                }
            };
            context.ActivePromotions = new List<BasePromotion>()
            {
                new BuyXGetYPromotion()
                {
                    Id=1,
                    BuyItems = new List<BuyPromotionItem>()
                    {
                        new BuyPromotionItem()
                        {
                            IdProduct=1,
                            QTY=2,
                        },
                    },
                    GetItems = new List<GetPromotionItem>()
                    {
                        new GetPromotionItem()
                        {
                            IdProduct=21,
                            QTY=2,
                            PercentDiscount=100,
                        },
                    }
                }
            };

            context.ActivePromotions = new List<BasePromotion>();
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(1, context.OrderToProducts.Count);
            Assert.True(context.OrderToProducts.FirstOrDefault(p => p.IdProduct == 21)==null);
        }
    }
}
