using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NLog;

namespace ExceptionDrivenDemo.Controllers {
    public class HomeController : Controller {
        private readonly Logger _logger = LogManager.GetLogger("HomeController");

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public ActionResult AddItemToOrder(int? productId) {
            if (!productId.HasValue || productId <= 0) {
                _logger.Log(LogLevel.Error, "Invalid ProductId");
                throw new ArgumentException("Invalid ProductId", "productId");
            }

            _logger.Log(LogLevel.Info, "Creating Order");
            var order = new Order();

            var item = new Item {
                Name = "Sample",
                Price = 9.99,
                Quantity = 1,
                ProductId = productId.Value
            };

            _logger.Log(LogLevel.Info, String.Format("Adding item with to order. ProductId: {0}, Quantity: {1}", item.ProductId, item.Quantity));
            order.LineItems.Add(item);

            _logger.Log(LogLevel.Info, "Calculating Total Price.");
            order.CalculateTotalPrice();

            _logger.Log(LogLevel.Info, "Calculated Total Price.");

            return View();
        }
    }

    public class Order {
        public Order() {
            CreatedDate = DateTime.Now;
            LineItems = new List<Item>();
        }

        public DateTime CreatedDate { get; set; }

        public List<Item> LineItems { get; set; }

        public void CalculateTotalPrice() {
            throw new Exception(Guid.NewGuid().ToString());
        }
    }

    public class Item {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}