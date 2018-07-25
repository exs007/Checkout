using CheckoutKataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Workflow.Orders
{
    //An interface for calculation total for an order during checkout
    public interface IOrderCalculationWorkflowProcessor
    {
        OrderCalculationContext CalculateOrder(OrderCalculationContext context);
    }
}
