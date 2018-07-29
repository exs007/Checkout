using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.Workflow.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Workflow.Orders.Actions
{
    /// <summary>
    /// Setup all promotions which are avaliable now
    /// </summary>
    public class SetupActivePromotionsAction : IWorkflowAction<OrderCalculationContext>
    {
        public void ExecuteAction(OrderCalculationContext context, IWorkflowProcessorContext processorContext)
        {
            var promotionService = processorContext.Resolve<IPromotionService>();

            context.ActivePromotions = promotionService.GetPromotions().Where(p=>
                    (!p.StartDate.HasValue || p.StartDate.Value <= DateTime.Now) &&
                    (!p.EndDate.HasValue || p.EndDate.Value >= DateTime.Now)).ToList();
        }
    }
}
