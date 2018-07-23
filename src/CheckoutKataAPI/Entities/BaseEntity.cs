using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutKataAPI.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public RecordStatusCode StatusCode { get; set; }
    }
}
