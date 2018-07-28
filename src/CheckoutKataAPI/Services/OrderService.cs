using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                throw new AppValidationException("Add product model isn't specififed");
            }

            var order = GetOrderFromRepository(idOrder);
            var product = GetProduct(item.SKU);

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
                throw new AppValidationException("The given product doesn't exist in the order");
            }

            if (orderToProductForDelete.IdUsedPromotion.HasValue)
            {
                throw new AppValidationException("Deleting promotion product isn't permitted");
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
                throw new AppValidationException("Invalid order id");
            }

            return order;
        }

        private Product GetProduct(string sku)
        {
            var product = _productService.GetProduct(sku);
            if (product==null)
            {
                throw new AppValidationException("Invalid product SKU");
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
