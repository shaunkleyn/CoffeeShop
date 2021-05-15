using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoffeeShop.Models.Entities
{
    public partial class Clients
    {
        public Clients()
        {
            Sales = new HashSet<Sales>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<Sales> Sales { get; set; }
    }
}
