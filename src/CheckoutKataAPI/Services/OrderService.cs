using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Constants;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Entities.Products;
using CheckoutKataAPI.Exceptions;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Workflow.Orders;

namespace CheckoutKataAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductService _productService;
        private readonly IOrderCalculationWorkflowProcessor _calculationProcessor;
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IProductService productService,
            IOrderCalculationWorkflowProcessor calculationProcessor,
            IRepository<Order> orderRepository)
        {
            _productService = productService;
            _calculationProcessor = calculationProcessor;
            _orderRepository = orderRepository;
        }

        public Order GetOrder(int id)
        {
            var order = GetOrderFromRepository(id);
            order = GetRecalculatedOrder(order);

            return order;
        }

        public Order CreateNewOrder()
        {
            var order = new Order();
            order = _orderRepository.Add(order);

            return order;
        }

        public Order AddProductInOrder(int idOrder, AddOrderToProductModel item)
        {
            if (item == null)
            {
                throw new AppValidationException(MessageConstants.ADD_PRODUCT_MODEL_IS_EMPTY);
            }

            var order = GetOrderFromRepository(idOrder);
            var product = GetProduct(item.SKU);
            if (product.PriceType == PriceType.PerEach && (item.QTY - Math.Floor(item.QTY))!=0)
            {
                throw new AppValidationException(MessageConstants.FRACTIONAL_QTY_NOT_AVALIABLE_IN_ORDER_FOR_PRODUCT_WITH_LB_PRICE);
            }

            var orderToProduct = order.OrderToProducts.FirstOrDefault(p=>p.IdProduct==product.Id);
            if (orderToProduct == null)
            {
                orderToProduct = new OrderToProduct()
                {
                    IdProduct=product.Id,
                };
                order.OrderToProducts.Add(orderToProduct);
            }
            orderToProduct.QTY += item.QTY;
            order = GetRecalculatedOrder(order);

            var result = _orderRepository.Update(order);
            return result ? order : null;
        }

        public Order DeleteProductInOrder(int idOrder, string sku)
        {
            var order = GetOrderFromRepository(idOrder);
            var product = GetProduct(sku);

            var orderToProductForDelete = order.OrderToProducts.FirstOrDefault(p=>p.IdProduct==product.Id);
            if (orderToProductForDelete == null)
            {
                throw new AppValidationException(MessageConstants.PRODUCT_NOT_EXIST_IN_ORDER);
            }

            if (orderToProductForDelete.IdUsedBuyGetPromotion.HasValue)
            {
                throw new AppValidationException(MessageConstants.DELETE_PROMO_PRODUCT_NOT_PERMITTED_IN_ORDER);
            }

            order.OrderToProducts.Remove(orderToProductForDelete);
            order = GetRecalculatedOrder(order);

            var result = _orderRepository.Update(order);
            return result ? order : null;
        }

        private Order GetOrderFromRepository(int id)
        {
            var order = _orderRepository.Select(id);
            if (order == null)
            {
                throw new AppValidationException(MessageConstants.NOT_FOUND_ORDER_ID);
            }

            return order;
        }

        private Product GetProduct(string sku)
        {
            var product = _productService.GetProduct(sku);
            if (product==null)
            {
                throw new AppValidationException(MessageConstants.NOT_FOUND_PRODUCT_BY_SKU_CODE);
            }

            return product;
        }

        private Order GetRecalculatedOrder(Order order)
        {
            var context = new OrderCalculationContext(order);
            context = _calculationProcessor.CalculateOrder(context);

            return context.GetResultOrder();
        }
    }
}
