using CoffeeShop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace CoffeeShop.Web.Controllers
{
    public class BaseController : Controller
    {
        public const string PRODUCTS = "products";
        public const string CLIENTS = "clients";
        public const string SALES = "sales";
        public const string SALES_PLACE_ORDER = @"sales\placeorder";

        protected readonly IConfiguration _config;
        protected readonly string _url;
        protected HttpClient client = new HttpClient();
        protected readonly CoffeeShopContext _context = new CoffeeShopContext();

        public BaseController(IConfiguration config)
        {
            _config = config;
            _url = _config.GetValue<string>("ApiUrl");
        }
    }
}
