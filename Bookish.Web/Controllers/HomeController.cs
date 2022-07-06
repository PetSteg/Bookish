using System.Diagnostics;
using Bookish.DataAccess;
using Bookish.DataAccess.Models;
using Bookish.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Bookish.Web.Models;
using Microsoft.Extensions.Logging;

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
        return View("Register");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string name, string email, string password)
    {
        db.InsertUser(name, email, password);
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        Console.WriteLine("User has cookies:");
        foreach (var cookie in Request.Cookies)
        {
            Console.WriteLine(cookie.Key + ":" + cookie.Value);
        }

        if (db.VerifyUser(email, password))
        {
            Response.Cookies.Append(
                "id",
                db.GetUserByEmail(email).Id_user.ToString(),
                new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    Secure = false
                }
            );
            Response.Redirect("/Home/Home");
        }

        return View();
    }

    [HttpGet]
    public IActionResult Home()
    {
        var userId = Request.Cookies["id"];
        if (userId == null)
        {
            Response.Redirect("/Home/Login");
        }

        Console.WriteLine("User has cookies:");
        foreach (var cookie in Request.Cookies)
        {
            Console.WriteLine(cookie.Key + ":" + cookie.Value);
        }

        var user = db.GetUserById(userId);
        var homeModel = new HomeModel();
        homeModel.Name = user.Name;
        homeModel.Email = user.Email;
        homeModel.Books = new List<BookModel>();

        foreach (var book in db.GetBooksBorrowedByUser(user.Id_user))
        {
            var dueDate = db.GetDueDateUserBook(user.Id_user, book.ISBN);
            homeModel.Books.Add(new BookModel(book, dueDate));
        }

        return View("Home", homeModel);
    }

    public IActionResult Library()
    {
        var library = new LibraryModel();
        library.Books = new List<BookModel>();

        foreach (var book in db.GetAllBooks())
        {
            var authors = db.GetAuthorsOfBook(book.ISBN);
            library.Books.Add(new BookModel(book, authors));
        }

        return View("Library", library);
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Borrowed(string ISBN)
    {
        
        var userId = Request.Cookies["id"];
        db.BorrowBook(ISBN, Int32.Parse(userId));
        Response.Redirect("/Home/Library");
        return View("Borrowed");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}