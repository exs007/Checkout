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
    public class SetupActivePromotionsActionTest
    {
        private readonly SetupActivePromotionsAction _action;

        public SetupActivePromotionsActionTest()
        {
            _action = new SetupActivePromotionsAction();
        }

        private IWorkflowProcessorContext GetExecutingContext(ICollection<BasePromotion> promotions)
        {
            var executingContextSetup = new Mock<IWorkflowProcessorContext>();
            var promotionServiceSetup = new Mock<IPromotionService>();
            promotionServiceSetup.Setup(p => p.GetPromotions()).Returns(promotions);

            executingContextSetup.Setup(p=>p.Resolve<IPromotionService>()).
                Returns(promotionServiceSetup.Object);
            return executingContextSetup.Object;
        }

        [Fact]
        public void SetupMultipleActivePromotionsAndCheckThatOnlyAvaliableOnThisTimeExists()
        {
            var context = new OrderCalculationContext(new Order()
            {
                Id=1,
            });
            var executingContext = GetExecutingContext(new List<BasePromotion>()
            {
                new PricePromotion()
                {
                    Id = 1,
                    StartDate = DateTime.Now.AddMonths(-1),
                    EndDate = DateTime.Now.AddMonths(1),
                },
                new PricePromotion()
                {
                    Id = 2,
                    StartDate = DateTime.Now.AddMonths(-1),
                    EndDate = null,
                },
                new PricePromotion()
                {
                    Id = 3,
                    StartDate = null,
                    EndDate = null,
                },
                new PricePromotion()
                {
                    Id = 4,
                    StartDate = DateTime.Now.AddMonths(1),
                    EndDate = null,
                },
                new PricePromotion()
                {
                    Id = 5,
                    StartDate = DateTime.Now.AddMonths(-2),
                    EndDate = DateTime.Now.AddMonths(-1),
                },
            });
            _action.ExecuteAction(context, executingContext);

            Assert.Equal(3, context.ActivePromotions.Count);
            Assert.Contains(context.ActivePromotions, p => p.Id == 1);
            Assert.Contains(context.ActivePromotions, p => p.Id == 2);
            Assert.Contains(context.ActivePromotions, p => p.Id == 3);
        }

        [Fact]
        public void SetupZeroActivePromotionsAndCheckCount()
        {
            var context = new OrderCalculationContext(new Order()
            {
                Id=1,
            });
            var executingContext = GetExecutingContext(new List<BasePromotion>());
            _action.ExecuteAction(context, executingContext);

            Assert.Equal(2, context.ActivePromotions.Count);
        }
    }
}
