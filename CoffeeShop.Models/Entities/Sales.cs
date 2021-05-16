using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace CoffeeShop.Models.Entities
{
    public partial class Sales
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int ProductId { get; set; }

        public decimal Price { get; set; }
        public Guid SaleIdentifier { get; set; }

        public virtual Clients Client { get; set; }
        public virtual Products Product { get; set; }
    }
}
