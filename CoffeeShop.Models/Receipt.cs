using CoffeeShop.Models.Entities;
using System;
using System.Collections.Generic;

namespace CoffeeShop.Models
{
    public class Receipt
    {
        public Guid SaleReference { get; set; }
        public Clients Client { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public decimal Total { get; set; }
        public int TotalItemsPurchased { get; set; }
    }
}
