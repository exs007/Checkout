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

namespace CheckoutKataAPI.Test.Workflow.Actions
{
    public class SetupOrderedProductsInfoActionTest
    {
        private readonly IWorkflowProcessorContext _executingContext;
        private readonly SetupOrderedProductsInfoAction _action;

        public SetupOrderedProductsInfoActionTest()
        {
            var executingContextSetup = new Mock<IWorkflowProcessorContext>();
            var productServiceSetup = new Mock<IProductService>();
            productServiceSetup.Setup(p => p.GetProducts(new List<int>() {1, 2}, false)).Returns(new List<Product>()
            {
                new Product()
                {
                    Id = 1,
                    Price = 10.5m,
                    PriceType = PriceType.PerQuantity
                },
                new Product()
                {
                    Id=2,
                    Price=15m,
                    PriceType =PriceType.PerLb
                },
            });
            productServiceSetup.Setup(p => p.GetProducts(new List<int>() {}, false)).Returns(new List<Product>()
            {
            });

            executingContextSetup.Setup(p=>p.Resolve<IProductService>()).
                Returns(productServiceSetup.Object);
            _executingContext = executingContextSetup.Object;
            _action = new SetupOrderedProductsInfoAction();
        }

        [Fact]
        public void ProcessOrderWithTwoOrderedProductAndCheckCorrectPriceAndAmount()
        {
            var context = new OrderCalculationContext(new Order()
            {
                Id=1,
                OrderToProducts = new List<OrderToProduct>()
                {
                    new OrderToProduct()
                    {
                        IdProduct=1,
                        QTY=2,
                    },
                    new OrderToProduct()
                    {
                        IdProduct=2,
                        QTY=5.5m
                    },
                }
            });
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(2, context.OrderToProducts.Count);
            Assert.Equal(10.5m, context.OrderToProducts.First(p => p.IdProduct == 1).Price);
            Assert.Equal(10.5m * 2, context.OrderToProducts.First(p => p.IdProduct == 1).Amount);
            Assert.Equal(15m, context.OrderToProducts.First(p => p.IdProduct == 2).Price);
            Assert.Equal(15m * 5.5m, context.OrderToProducts.First(p => p.IdProduct == 2).Amount);
        }

        [Fact]
        public void ProcessOrderWithTwoOrderedProductAndOnePromoProductAndCheckCountInContext()
        {
            var context = new OrderCalculationContext(new Order()
            {
                Id=1,
                OrderToProducts = new List<OrderToProduct>()
                {
                    new OrderToProduct()
                    {
                        IdProduct=1,
                        QTY=2,
                    },
                    new OrderToProduct()
                    {
                        IdProduct=2,
                        QTY=5.5m
                    },
                    new OrderToProduct()
                    {
                        IdProduct=5,
                        QTY=5,
                        IdUsedBuyGetPromotion =10
                    },
                }
            });
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(2, context.OrderToProducts.Count);
            Assert.Contains(context.OrderToProducts, p => p.IdProduct == 1);
            Assert.Contains(context.OrderToProducts, p => p.IdProduct == 2);
        }

        [Fact]
        public void ProcessOrderWittZerosProductsAndCheckCountInContext()
        {
            var context = new OrderCalculationContext(new Order()
            {
                Id=1,
            });
            _action.ExecuteAction(context, _executingContext);

            Assert.Equal(0, context.OrderToProducts.Count);
        }
    }
}
