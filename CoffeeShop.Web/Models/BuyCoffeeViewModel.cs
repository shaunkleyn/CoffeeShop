using CoffeeShop.Models;
using CoffeeShop.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CoffeeShop.Web.Models
{
    public class BuyCoffeeViewModel :Order
    {
        public List<Products> Coffees { get; set; }
        public List<SelectListItem> Clients { get; set; }
    }
}
