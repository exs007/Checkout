﻿using CheckoutKataAPI.Workflow.Base;
using CheckoutKataAPI.Workflow.Orders.Actions;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Entities.Orders;
using Xunit;
using CheckoutKataAPI.Services;
using CheckoutKataAPI.DAL;
using CheckoutKataAPI.Test.DAL;
using CheckoutKataAPI.Entities.Products;
using CheckoutKataAPI.Workflow.Orders;
using CheckoutKataAPI.Exceptions;
using CheckoutKataAPI.Constants;

namespace CheckoutKataAPI.Test.Services
{
    public class OrderServiceTest
    {
        private readonly IOrderService _orderService;
        private readonly IRepository<Order> _orderRepository;
        private readonly Product _existProductPricePerLb = new Product()
        {
            Id = 1,
            SKU = "sku1",
            Price = 10m,
            PriceType = PriceType.PerLb
        };
        private readonly Product _existProductPricePerEach = new Product()
        {
            Id = 3,
            SKU = "sku3",
            Price = 21m,
            PriceType = PriceType.PerEach
        };

        private readonly string _notExistProductSku = "sku2";

        public OrderServiceTest()
        {
            _orderRepository = new FakeRepository<Order>();

            var productServiceSetup = new Mock<IProductService>();
            productServiceSetup.Setup(p=>p.GetProduct(_existProductPricePerLb.SKU)).
                Returns(_existProductPricePerLb);
            productServiceSetup.Setup(p=>p.GetProduct(_existProductPricePerEach.SKU)).
                Returns(_existProductPricePerEach);
            productServiceSetup.Setup(p=>p.GetProduct(_notExistProductSku)).
                Returns((Product)null);

            var calculationProcessorSetup = new Mock<IOrderCalculationWorkflowProcessor>();
            calculationProcessorSetup.Setup(p => p.CalculateOrder(It.IsAny<OrderCalculationContext>()))
                .Returns((OrderCalculationContext p) =>
                {
                    p.OrderToProducts = p.SourceOrder.OrderToProducts;
                    return p;
                });

            _orderService = new OrderService(productServiceSetup.Object, calculationProcessorSetup.Object,
                _orderRepository);
        }

        [Fact]
        public void CreateNewOrderAndCheckExistInRepository()
        {
            var order = _orderService.CreateNewOrder();
            var orderFromRepository = _orderRepository.Select(order.Id);

            Assert.NotNull(orderFromRepository);
        }

        [Fact]
        public void GetExistOrderFromService()
        {
            var order = _orderRepository.Add(new Order());
            var orderFromService = _orderService.GetOrder(order.Id);

            Assert.NotNull(orderFromService);
        }

        [Fact]
        public void TryToGetNotExistOrderFromServiceAndThrowException()
        {
            var exception = Assert.ThrowsAny<AppValidationException>(()=> _orderService.GetOrder(1000));
            Assert.Contains(MessageConstants.NOT_FOUND_ORDER_ID, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void AddExistInStoreProductToNotExistOrderAndThrowException()
        {
            var order = _orderRepository.Add(new Order());

            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.AddProductInOrder(order.Id,
                new AddOrderToProductModel()
                {
                    SKU = _notExistProductSku,
                    QTY = 1,
                }));
            Assert.Contains(MessageConstants.NOT_FOUND_PRODUCT_BY_SKU_CODE, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void AddNotExistInStoreProductToExistOrderAndThrowException()
        {
            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.AddProductInOrder(1000,
                new AddOrderToProductModel()
                {
                    SKU = _notExistProductSku,
                    QTY = 1,
                }));
            Assert.Contains(MessageConstants.NOT_FOUND_ORDER_ID, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void AddNullModelToExistOrderAndThrowException()
        {
            var order = _orderRepository.Add(new Order());

            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.AddProductInOrder(order.Id,
                null));
            Assert.Contains(MessageConstants.ADD_PRODUCT_MODEL_IS_EMPTY, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void AddExistInStoreProductToExistOrderWithTheSameProductAndCheckThatQTYWasCorrectlyChanged()
        {
            var order = _orderRepository.Add(new Order());
            order.OrderToProducts.Add(new OrderToProduct()
            {
                IdProduct=_existProductPricePerLb.Id,
                QTY=1m,
            });

            var resultOrder = _orderService.AddProductInOrder(order.Id, new AddOrderToProductModel()
            {
                SKU = _existProductPricePerLb.SKU,
                QTY = 1m,
            });

            Assert.Equal(2m, resultOrder.OrderToProducts.First().QTY);
        }

        [Fact]
        public void AddExistInStoreProductToExistOrderWithoutProductsAndCheckProductWasAddedWithCorrectQTY()
        {
            var order = _orderRepository.Add(new Order());

            var resultOrder = _orderService.AddProductInOrder(order.Id, new AddOrderToProductModel()
            {
                SKU = _existProductPricePerLb.SKU,
                QTY = 1.5m,
            });
            
            Assert.Equal(1, resultOrder.OrderToProducts.Count);
            Assert.Equal(1.5m, resultOrder.OrderToProducts.First().QTY);
        }
        
        [Fact]
        public void DeleteNotExistInStoreProductFromExistOrderAndThrowException()
        {
            var order = _orderRepository.Add(new Order());

            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.DeleteProductInOrder(order.Id,
                _notExistProductSku));
            Assert.Contains(MessageConstants.NOT_FOUND_PRODUCT_BY_SKU_CODE, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void DeleteExistInStoreProductFromNotExistOrderAndThrowException()
        {
            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.DeleteProductInOrder(1000,
                _existProductPricePerLb.SKU));
            Assert.Contains(MessageConstants.NOT_FOUND_ORDER_ID, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void DeleteExistInStoreProductFromExistOrderWhichDontContainsTheGivenProductAndThrowException()
        {
            var order = _orderRepository.Add(new Order());

            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.DeleteProductInOrder(order.Id,
                _existProductPricePerLb.SKU));
            Assert.Contains(MessageConstants.PRODUCT_NOT_EXIST_IN_ORDER, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void DeleteExistInStoreProductFromExistOrderWhichContainsTheGivenProductAsPromoProductAndThrowException()
        {
            var order = _orderRepository.Add(new Order());
            order.OrderToProducts.Add(new OrderToProduct()
            {
                IdProduct=_existProductPricePerLb.Id,
                QTY=1m,
                IdUsedBuyGetPromotion=11,
            });

            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.DeleteProductInOrder(order.Id,
                _existProductPricePerLb.SKU));
            Assert.Contains(MessageConstants.DELETE_PROMO_PRODUCT_NOT_PERMITTED_IN_ORDER, exception.Messages.Select(p => p.Message));
        }

        [Fact]
        public void DeleteExistInStoreProductFromExistOrderAndCheckThatItWasRemovedFromOrder()
        {
            var order = _orderRepository.Add(new Order());
            order.OrderToProducts.Add(new OrderToProduct()
            {
                IdProduct=_existProductPricePerLb.Id,
                QTY=1m,
            });

            order = _orderService.DeleteProductInOrder(order.Id, _existProductPricePerLb.SKU);
            Assert.Equal(0, order.OrderToProducts.Count);
        }

        [Fact]
        public void AddInOrderProductWithPricePerEachWithNotWholeQTYAndThrowException()
        {
            var order = _orderRepository.Add(new Order());

            var exception = Assert.ThrowsAny<AppValidationException>(() => _orderService.AddProductInOrder(order.Id,
                new AddOrderToProductModel()
                {
                    SKU = _existProductPricePerEach.SKU,
                    QTY = 1.5m,
                }));
            Assert.Contains(MessageConstants.FRACTIONAL_QTY_NOT_AVALIABLE_IN_ORDER_FOR_PRODUCT_WITH_LB_PRICE,
                exception.Messages.Select(p => p.Message));
        }
    }
}
