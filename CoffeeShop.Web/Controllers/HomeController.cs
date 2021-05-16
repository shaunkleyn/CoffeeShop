using CoffeeShop.Common;
using CoffeeShop.Models;
using CoffeeShop.Models.Entities;
using CoffeeShop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeShop.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration config) : base(config)
        {
        }

        public async Task<IActionResult> Index()
        {
            BuyCoffeeViewModel model = new BuyCoffeeViewModel();
            model.Coffees = JsonConvert.DeserializeObject<List<Products>>(await client.GetStringAsync(_url.AppendToURL(PRODUCTS))).ToList();
            model.Coffees.ForEach(x => model.OrderItems.Add(new OrderItems() 
            {  
                ProductId = x.Id, 
                ProductName = x.Name, 
                Quantity = 0,
                Price = x.Price
            }));
            //var clients = JsonConvert.DeserializeObject<List<Clients>>(await client.GetStringAsync(_url.AppendToURL(CLIENTS))).ToList();
            //model.Clients = clients.Select(x => new SelectListItem(x.EmailAddress, x.Id.ToString())).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(BuyCoffeeViewModel model)
        {
            Order order = new Order()
            {
                OrderItems = model.OrderItems,
                ClientEmailAddress = model.ClientEmailAddress
            };

            var result = await client.PostAsJsonAsync<Receipt>(_url.AppendToURL(SALES_PLACE_ORDER), order);
            return View("Receipt", result);
        }

        public async Task<IActionResult> Clients()
        {
            var model = JsonConvert.DeserializeObject<List<Clients>>(await client.GetStringAsync(_url.AppendToURL(CLIENTS))).ToList();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
