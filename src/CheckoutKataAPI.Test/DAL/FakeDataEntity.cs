using System;
using System.Collections.Generic;
using System.Text;
using CheckoutKataAPI.Entities;

namespace CheckoutKataAPI.Test.DAL
{
    internal class FakeDataEntity : BaseEntity
    {
        public string StringData { get; set; }

        public int IntData { get; set; }
    }
}
