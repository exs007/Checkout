using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckoutKataAPI.Entities;
using CheckoutKataAPI.Entities.Orders;
using CheckoutKataAPI.Entities.Products;
using CheckoutKataAPI.Models;

namespace CheckoutKataAPI.Services
{
    public interface IOrderService
    {
        Order GetOrder(int sku);

        Order CreateNewOrder();

        Order AddProductInOrder(int idOrder, AddOrderToProductModel item);

        Order DeleteProductInOrder(int idOrder, string sku);
    }
}
