using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Entities.Products;
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

        public Order GetOrder(int sku)
        {
            throw new NotImplementedException();
        }

        public Order CreateNewOrder()
        {
            throw new NotImplementedException();
        }

        public Order AddProductInOrder(int idOrder, AddOrderToProductModel item)
        {
            throw new NotImplementedException();
        }

        public Order DeleteProductInOrder(int idOrder, string sku)
        {
            throw new NotImplementedException();
        }
    }
}
