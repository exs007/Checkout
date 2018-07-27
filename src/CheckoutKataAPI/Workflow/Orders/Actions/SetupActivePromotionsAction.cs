﻿using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.Workflow.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Workflow.Orders.Actions
{
    //setup all promotions which avaliable now
    public class SetupActivePromotionsAction : IWorkflowAction<OrderCalculationContext>
    {
        public void ExecuteAction(OrderCalculationContext context, IWorkflowProcessorContext processorContext)
        {

        }
    }
}