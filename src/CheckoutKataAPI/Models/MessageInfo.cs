using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckoutKataAPI.Models
{
    public class MessageInfo
    {
        public string Field { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"[({Field}){Message}";
        }
    }
}