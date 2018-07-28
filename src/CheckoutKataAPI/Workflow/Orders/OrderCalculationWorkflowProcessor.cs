using CheckoutKataAPI.Models;
using CheckoutKataAPI.Workflow.Base;
using CheckoutKataAPI.Workflow.Orders.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Workflow.Orders
{
    public class OrderCalculationWorkflowProcessor : WorkflowProcessor<OrderCalculationContext>, IOrderCalculationWorkflowProcessor
    {
        public OrderCalculationWorkflowProcessor(IServiceProvider provider) : base(provider)
        {
            SetupAction<SetupOrderedProductsInfoAction>();
            SetupAction<SetupActivePromotionsAction>();
            SetupAction<BuyXGetYPromotionsAction>();
            SetupAction<PricePromotionsAction>();
        }

        public OrderCalculationContext CalculateOrder(OrderCalculationContext context)
        {
            return Execute(context);
        }
    }
}
