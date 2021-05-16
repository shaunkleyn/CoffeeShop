using CoffeeShop.Models;
using CoffeeShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        public const string CASH_SALE_EMAIL = "Cash Sale";
        private readonly CoffeeShopContext _context;

        public SalesController(CoffeeShopContext context)
        {
            _context = context;
        }


        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sales>>> GetSales()
        {
            return await _context.Sales.ToListAsync();
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sales>> GetSales(int id)
        {
            var sales = await _context.Sales.FindAsync(id);

            if (sales == null)
            {
                return NotFound();
            }

            return sales;
        }

        // PUT: api/Sales/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSales(int id, Sales sales)
        {
            if (id != sales.Id)
            {
                return BadRequest();
            }

            _context.Entry(sales).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sales
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Sales>> PostSales(Sales sales)
        {
            _context.Sales.Add(sales);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SalesExists(sales.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSales", new { id = sales.Id }, sales);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sales>> DeleteSales(int id)
        {
            var sales = await _context.Sales.FindAsync(id);
            if (sales == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sales);
            await _context.SaveChangesAsync();

            return sales;
        }

        private bool SalesExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }


        [HttpPost]
        [Route("placeorder")]
        public async Task<Receipt> PlaceOrder(Order model)
        {
            Receipt receipt = new Receipt();
            Guid salesIdentifier = Guid.NewGuid();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (string.IsNullOrEmpty(model.ClientEmailAddress))
                    {
                        model.ClientEmailAddress = CASH_SALE_EMAIL;
                    }

                    var client = _context.Clients.FirstOrDefault(x => x.EmailAddress.Equals(model.ClientEmailAddress.Trim()));
                    
                    // this customer hasnt bought from us before so let's add him to the clients table
                    if (client == null)
                    {
                        client = new Clients()
                        {
                            EmailAddress = model.ClientEmailAddress.Trim().ToLowerInvariant()
                        };

                        _context.Clients.Add(client);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var orderItem in model.OrderItems.Where(x => x.Quantity > 0))
                    {
                        orderItem.Price = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id.Equals(orderItem.ProductId))?.Price ?? 0M;
                        for (int i = 0; i < orderItem.Quantity; i++)
                        {
                           var sale = new Sales()
                            {
                                TransactionDate = DateTime.Now,
                                ClientId = client.Id,
                                ProductId = orderItem.ProductId,
                                SaleIdentifier = salesIdentifier,
                                Price = orderItem.Price
                            };

                            _context.Sales.Add(sale);
                            await _context.SaveChangesAsync();
                        }
                    }

                    transaction.Commit();

                    receipt.Client = client;
                    receipt.OrderItems = model.OrderItems;
                    receipt.SaleReference = salesIdentifier;
                    receipt.Total = model.OrderItems.Sum(x => x.Price);
                    
                    if(!model.ClientEmailAddress.Equals(CASH_SALE_EMAIL, StringComparison.OrdinalIgnoreCase))
                    {
                        receipt.TotalItemsPurchased = _context.Sales.Count(x => x.ClientId == client.Id);
                    }
                }
                catch (Exception)
                {

                    transaction.Rollback();
                }
            }

            return receipt;
        }
    }
}
