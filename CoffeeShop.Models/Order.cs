using System.Collections.Generic;

namespace CoffeeShop.Models
{
    public class Order
    {
        public int ClientId { get; set; }
        public string ClientEmailAddress { get; set; }
        public List<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}
