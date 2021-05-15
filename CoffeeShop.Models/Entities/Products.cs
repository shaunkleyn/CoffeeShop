using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoffeeShop.Models.Entities
{
    public partial class Products
    {
        public Products()
        {
            Sales = new HashSet<Sales>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<Sales> Sales { get; set; }
    }
}
