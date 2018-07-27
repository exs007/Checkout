using CheckoutKataAPI.Models;
using CheckoutKataAPI.Workflow.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Workflow.Orders.Actions
{
    //setup price info about products in an order which were bought without promotions
    public class SetupOrderedProductsInfoAction : IWorkflowAction<OrderCalculationContext>
    {
        public void ExecuteAction(OrderCalculationContext context, IWorkflowProcessorContext processorContext)
        {
            throw new NotImplementedException();
        }
    }
}
