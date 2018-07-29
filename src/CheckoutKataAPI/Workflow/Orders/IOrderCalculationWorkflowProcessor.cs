using CheckoutKataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Workflow.Orders
{
    /// <summary>
    /// An interface for calculating an order 
    /// </summary>
    public interface IOrderCalculationWorkflowProcessor
    {
        OrderCalculationContext CalculateOrder(OrderCalculationContext context);
    }
}
