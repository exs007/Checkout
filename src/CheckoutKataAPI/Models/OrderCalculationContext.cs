using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Entities.Promotions;
using CheckoutKataAPI.Workflow.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Models
{
    public class OrderCalculationContext : WorkflowDataContext
    {
        // an order for calculation in a context
        public Order SourceOrder { get; }

        // promotions which can be applied to a calculation process
        public ICollection<BasePromotion> ActivePromotions {get;set;}

        // result list of products with price info
        public ICollection<OrderToProduct> OrderToProducts {get;set;}
        
        // result order's total
        public decimal Total => OrderToProducts.Sum(p => p.Amount);

        public OrderCalculationContext(Order sourceOrder)
        {
            SourceOrder = sourceOrder;
            ActivePromotions = new List<BasePromotion>();
            OrderToProducts = new List<OrderToProduct>();
        }

        public Order GetResultOrder()
        {
            var toReturn = new Order();
            toReturn.Id = SourceOrder.Id;
            toReturn.StatusCode = SourceOrder.StatusCode;
            toReturn.OrderToProducts = OrderToProducts;
            toReturn.Total = Total;

            return toReturn;
        }
    }
}
