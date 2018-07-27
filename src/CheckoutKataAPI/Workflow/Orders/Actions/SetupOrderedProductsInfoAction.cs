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
    //setup price info about products in an order which were bought without promotions
    public class SetupOrderedProductsInfoAction : IWorkflowAction<OrderCalculationContext>
    {
        public void ExecuteAction(OrderCalculationContext context, IWorkflowProcessorContext processorContext)
        {
            var productsInOrderWithoutPromotions = context.SourceOrder.OrderToProducts.
                Where(p => !p.IdUsedPromotion.HasValue).ToList();

            var productService = processorContext.Resolve<IProductService>();
            var products = productService.GetProducts(productsInOrderWithoutPromotions.Select(p=>p.IdProduct).ToList());

            foreach (var item in productsInOrderWithoutPromotions)
            {
                var product = products.FirstOrDefault(p => p.Id == item.IdProduct);
                if (product != null)
                {
                    var orderToProduct = new OrderToProduct();
                    orderToProduct.IdProduct = item.IdProduct;
                    orderToProduct.QTY = item.QTY;
                    orderToProduct.Price = product.Price;
                    context.OrderToProducts.Add(orderToProduct);
                }
            }
        }
    }
}
