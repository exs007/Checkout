using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Models;
using CheckoutKataAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutKataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseApiController 
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public Result<Order> Post()
        {
            return _orderService.CreateNewOrder();
        }

        [HttpGet("{id}")]
        public Result<Order> Get(int id)
        {
            return _orderService.GetOrder(id);
        }

        [HttpPost("{id:int:min(1)}/products")]
        public Result<Order> AddProductInOrder(int id, [FromBody]AddOrderToProductModel model)
        {
            return _orderService.AddProductInOrder(id, model);
        }

        [HttpDelete("{id:int:min(1)}/products/{sku}")]
        public Result<Order> DeleteProductInOrder(int id, string sku)
        {
            return _orderService.DeleteProductInOrder(id, sku);
        }
    }
}
