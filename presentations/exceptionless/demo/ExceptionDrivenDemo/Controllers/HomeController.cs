using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
            return View(order);
        }

        private static Task<Item> GetProductLineItemAsync(int productId) {
            Item item;
            switch (productId) {
                case 1:
                    item = new Item {
                        Name = "Car",
                        Price = 9.99,
                        Quantity = 1,
                        ProductId = productId
                    };
                    break;
                case 2:
                    item = new Item {
                        Name = "Truck",
                        Price = 9.99,
                        Quantity = 1,
                        ProductId = productId
                    };
                    break;
                default:
                    throw new Exception($"Product not found: {productId}");
            }

            return Task.FromResult(item);
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

        public double SalesTax { get; set;  }

        public void CalculateTotalPrice() {
            Price = LineItems.Sum(i => i.Price * i.Quantity);
            SalesTax = Price * GetSalesTax();
        }

        private double GetSalesTax() {
            throw new NotImplementedException();
        }
    }

    public class Item {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}