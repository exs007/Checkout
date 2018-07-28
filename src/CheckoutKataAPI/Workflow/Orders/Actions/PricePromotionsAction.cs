using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Entities.Promotions;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.Workflow.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Workflow.Orders.Actions
{
    // apply price rules for specific promotions
    public class PricePromotionsAction : IWorkflowAction<OrderCalculationContext>
    {
        public void ExecuteAction(OrderCalculationContext context, IWorkflowProcessorContext processorContext)
        {
            var pricePromotions = context.ActivePromotions.Where(p => p is PricePromotion)
                .Select(p => (PricePromotion) p).ToList();
            foreach (var orderToProduct in context.OrderToProducts.Where(p=>!p.IdUsedBuyGetPromotion.HasValue))
            {
                var biggestPromotion = pricePromotions.Where(p => p.AssignedProductIds.Contains(orderToProduct.IdProduct))
                    .OrderByDescending(p => p.PriceDiscount).FirstOrDefault();

                if (biggestPromotion != null)
                {
                    orderToProduct.Price = Math.Max(orderToProduct.Price - biggestPromotion.PriceDiscount, 0);
                }
            }
        }
    }
}
