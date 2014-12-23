using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobimarket.Controllers
{
    public class OrderModel
    {
        public List<OrderItem> orderItems = new List<OrderItem>();

        public DateTime Date { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
    }
}
