using System.Diagnostics;
using Bookish.DataAccess;
using Bookish.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Bookish.Web.Models;

namespace simpleForm.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static Database db = new Database();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string name, string email, string password)
    {
        Console.WriteLine(name + "; " + email + "; " + password);
        db.InsertUser(name, email, password);
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}