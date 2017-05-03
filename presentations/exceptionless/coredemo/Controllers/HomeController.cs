using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Fluent;
using Exceptionless;

namespace coredemo.Controllers
{
  public class HomeController : Controller
  {
    private readonly Logger _logger = LogManager.GetLogger("HomeController");

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult About()
    {
      ViewData["Message"] = "Your application description page.";
      return View();
    }

    public IActionResult Send100Errors()
    {
      for (int i = 0; i < 100; i++)
      {
        ExceptionlessClient.Default.CreateException(new Exception($"Error {i}")).SetUserIdentity($"user{i}").Submit();
      }
      return Redirect("/");
    }

    public IActionResult Add100Users()
    {
      for (int i = 0; i < 100; i++)
      {
        ExceptionlessClient.Default.CreateSessionStart().SetUserIdentity($"user{i}").Submit();
      }
      return Redirect("/");
    }

    public IActionResult Contact()
    {
      _logger.Trace().Message($"GET /Home/Contact").Write();

      ViewData["Message"] = "Your contact page.";

      return View();
    }

    public IActionResult Error()
    {
      return View();
    }

    public IActionResult FileNotFound()
    {
      System.IO.File.ReadAllText("missing");

      return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddItemToOrder(int? productId)
    {
      if (!productId.HasValue || productId <= 0)
      {
        _logger.Error().Message("Invalid ProductId");
        throw new ArgumentException("Invalid ProductId", nameof(productId));
      }

      _logger.Info().Message("Creating Order").Write();
      var order = new Order();

      var item = await GetProductLineItemAsync(productId.Value);
      _logger.Info().Message("Adding item to order.").Write();
      order.LineItems.Add(item);

      _logger.Info().Message("Calculating Total Price.").Write();
      order.CalculateTotalPrice();

      _logger.Info().Message("Calculated Total Price.").Write();
      return View(order);
    }

    private static Task<Item> GetProductLineItemAsync(int productId)
    {
      Item item;
      switch (productId)
      {
        case 1:
          item = new Item
          {
            Name = "Car",
            Price = 9.99,
            Quantity = 1,
            ProductId = productId
          };
          break;
        case 2:
          item = new Item
          {
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

  public class Order
  {
    public Order()
    {
      CreatedDate = DateTime.Now;
      LineItems = new List<Item>();
    }

    public DateTime CreatedDate { get; set; }

    public List<Item> LineItems { get; set; }

    public double Price { get; set; }

    public double SalesTax { get; set; }

    public void CalculateTotalPrice()
    {
      Price = LineItems.Sum(i => i.Price * i.Quantity);
      SalesTax = Price * GetSalesTax();
    }

    private double GetSalesTax()
    {
      throw new NotImplementedException();

      // oops, should return 0.055;
    }
  }

  public class Item
  {
    public int ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
  }

}
