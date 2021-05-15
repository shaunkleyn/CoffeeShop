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
                    List<Sales> sales = new List<Sales>();
                    foreach (var orderItem in model.OrderItems)
                    {
                        for (int i = 0; i < orderItem.Quantity; i++)
                        {

                           var sale = new Sales()
                            {
                                TransactionDate = DateTime.Now,
                                ClientId = model.ClientId,
                                ProductId = orderItem.ProductId,
                                SaleIdentifier = salesIdentifier,
                                Price = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id.Equals(orderItem.ProductId))?.Price ?? 0M
                            };

                            orderItem.Price = sale.Price;
                            _context.Sales.Add(sale);
                            await _context.SaveChangesAsync();
                        }
                    }

                    transaction.Commit();

                    var result = _context.Sales.Where(x => x.SaleIdentifier == salesIdentifier).ToList();
                    var client = _context.Clients.FirstOrDefault(x => x.Id == model.ClientId);
                    var products = _context.Products.AsEnumerable();
                    result.ForEach(x => 
                    {
                        x.Client = client ?? new Clients() { Name = "Cash Sale"};
                        x.Product = products.FirstOrDefault(y => y.Id == x.ProductId);
                    });

                    receipt.Client = _context.Clients.FirstOrDefault(x => x.Id == model.ClientId);
                    receipt.OrderItems = model.OrderItems;
                    receipt.SaleReference = salesIdentifier;
                    receipt.Total = result.Sum(x => x.Price);
                    if(model.ClientId > 0)
                    {

                        receipt.TotalItemsPurchased = _context.Sales.Count(x => x.ClientId == model.ClientId);
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
