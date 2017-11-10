using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exceptionless;
using Microsoft.Extensions.Logging;

namespace coredemo.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger _logger;
    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

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
      _logger.LogTrace("GET /Home/Contact");

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
        _logger.LogError("Invalid ProductId");
        throw new ArgumentException("Invalid ProductId", nameof(productId));
      }

      _logger.LogInformation("Creating Order");
      var order = new Order();

      try
      {
        var item = await GetProductLineItemAsync(productId.Value);
        _logger.LogInformation("Adding item to {Order}.", order);
        order.LineItems.Add(item);

        _logger.LogInformation("Calculating Total Price for {Order}.", order);
        order.CalculateTotalPrice();

        _logger.LogInformation("Calculated Total Price for {Order}.", order);
        return View(order);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error processing order for {Order}.", order);
        throw;
      }
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
