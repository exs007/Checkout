using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Entities.Products;
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
    /// <summary>
    /// Add promotion products into the context by promo rules
    /// </summary>
    public class BuyXGetYPromotionsAction : IWorkflowAction<OrderCalculationContext>
    {
        public void ExecuteAction(OrderCalculationContext context, IWorkflowProcessorContext processorContext)
        {
            var buyGetPromotions = context.ActivePromotions.Where(p => p is BuyXGetYPromotion)
                .Select(p => (BuyXGetYPromotion) p);
            var notPromoProducts = context.OrderToProducts.ToList();
            var appliedPromotionInfos = new List<Tuple<BuyXGetYPromotion, int>>();

            foreach (var promotion in buyGetPromotions)
            {
                var useCount = GetPossiableNumberOfUse(promotion, notPromoProducts);
                if (promotion.ApplyLimit.HasValue)
                {
                    useCount = Math.Min(useCount, promotion.ApplyLimit.Value);
                }

                if (useCount > 0)
                {
                    appliedPromotionInfos.Add(new Tuple<BuyXGetYPromotion, int>(promotion, useCount));
                }
            }

            if (appliedPromotionInfos.Count > 0)
            {
                var productIds = appliedPromotionInfos.SelectMany(p => p.Item1.GetItems).Select(p=>p.IdProduct).ToList();
                var productService = processorContext.Resolve<IProductService>();
                var products = productService.GetProducts(productIds, true);
                SetPricesForPromoProducts(context, products, appliedPromotionInfos);
            }
        }
        
        // We use AND logic for calculating count of possiable usages based on all buy items
        private int GetPossiableNumberOfUse(BuyXGetYPromotion promotion,IEnumerable<OrderToProduct> notPromoProducts)
        {
            var toReturn = 0;
            foreach (var buyItem in promotion.BuyItems)
            {
                var productRecord = notPromoProducts.FirstOrDefault(p=>p.IdProduct==buyItem.IdProduct);
                if (productRecord == null)
                {
                    toReturn = 0;
                    break;
                }

                var maxUseForBuyItem = (int)Math.Floor(productRecord.QTY / buyItem.QTY);
                toReturn = toReturn == 0 ? maxUseForBuyItem : Math.Min(toReturn, maxUseForBuyItem);
            }

            return toReturn;
        }

        //Set prices based on products from data source and promotion rules
        private void SetPricesForPromoProducts(OrderCalculationContext context, IEnumerable<Product> products,
            IEnumerable<Tuple<BuyXGetYPromotion, int>> appliedPromotionInfos)
        {
            foreach (var item in appliedPromotionInfos)
            {
                foreach (var getItem in item.Item1.GetItems)
                {
                    var sourceProduct = products.FirstOrDefault(p=>p.Id==getItem.IdProduct);
                    if (sourceProduct == null)
                        continue;

                    var orderToProduct = new OrderToProduct();
                    orderToProduct.IdProduct = getItem.IdProduct;
                    orderToProduct.IdUsedBuyGetPromotion = item.Item1.Id;
                    // applied time * qty from getItem
                    orderToProduct.QTY = item.Item2 * getItem.QTY;
                    orderToProduct.Price = Math.Round(sourceProduct.Price * (100 - getItem.PercentDiscount) / 100, 2);

                    context.OrderToProducts.Add(orderToProduct);
                }
            }
        }
    }
}
