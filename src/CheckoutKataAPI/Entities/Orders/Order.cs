﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities.Orders
{
    public class Order
    {
        public int Id { get; set; }

        public ICollection<OrderToProduct> OrderToProducts {get;set;}

        public decimal Total {get;set;}

        public Order()
        {
            OrderToProducts = new List<OrderToProduct>();
        } 
    }
}
