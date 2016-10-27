using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog.Fluent;

namespace ExceptionDrivenDemo.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            Log.Trace().Message("Loading view").Write();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToOrder(int? productId) {
            if (!productId.HasValue || productId <= 0) {
                Log.Error().Message("Invalid ProductId").Write();
                throw new ArgumentException("Invalid ProductId", nameof(productId));
            }

            Log.Info().Message("Creating Order").Write();
            var order = new Order();

            var item = await GetProductLineItemAsync(productId.Value);
            Log.Info().Message("Adding item with to order.").Property("Product", item).Write();
            order.LineItems.Add(item);

            Log.Info().Message("Calculating Total Price.").Write();
            order.CalculateTotalPrice();

            Log.Info().Message("Calculated Total Price.").Write();
            return View();
        }

        private static Task<Item> GetProductLineItemAsync(int productId) {
            return Task.FromResult(new Item {
                Name = "Sample",
                Price = 9.99,
                Quantity = 1,
                ProductId = productId
            });
        }
    }

    public class Order {
        public Order() {
            CreatedDate = DateTime.Now;
            LineItems = new List<Item>();
        }

        public DateTime CreatedDate { get; set; }

        public List<Item> LineItems { get; set; }

        public double Price { get; set; }

        public double SalesTax { get; set; }

        public void CalculateTotalPrice() {
            Price = LineItems.Sum(i => i.Price * i.Quantity);
            SalesTax = Price / 0;
        }
    }

    public class Item {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}