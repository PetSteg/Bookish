using System.Diagnostics;
using Bookish.DataAccess;
using Bookish.DataAccess.Models;
using Bookish.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookish.Web.Controllers;

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
        var registerModel = new RegisterModel();
        registerModel.Error = false;
        return View(registerModel);
    }

    [HttpPost]
    public IActionResult Register(string name, string email, string password)
    {
        if (db.InsertUser(name, email, password))
        {
            return Redirect("/Home/Login");
        }

        var registerModel = new RegisterModel { Error = true };
        return View(registerModel);
    }

    [HttpGet]
    public IActionResult AddBook()
    {
        var userId = Request.Cookies["id"];

        if (userId == null)
        {
            Response.Redirect("/Home/Login");
        }

        return View();
    }

    [HttpPost]
    public IActionResult AddBook(BookModel bookModel)
    {
        var userId = Request.Cookies["id"];

        if (userId == null)
        {
            return View();
        }

        var authors = bookModel.AuthorsString.Split(',').ToList();

        db.InsertBook(bookModel.Book, authors);
        
        var library = new LibraryModel();
        library.Books = new List<BookModel>();
        
        library.Books.Add(bookModel);
        
        return View("Library", library);
    }

    [HttpGet]
    public IActionResult Login()
    {
        var loginModel = new LoginModel { Error = false };
        return View(loginModel);
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
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
            // Response.Redirect("/Home/Home");
            return Redirect("/Home/Home");
        }

        var loginModel = new LoginModel { Error = true };
        return View(loginModel);
    }

    [HttpGet]
    public IActionResult Home()
    {
        var userId = Request.Cookies["id"];
        if (userId == null)
        {
            Response.Redirect("/Home/Login");
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

    private LibraryModel MakeLibraryAtPage(List<Book> books, int page)
    {
        var library = new LibraryModel { Books = new List<BookModel>() };

        foreach (var book in books)
        {
            if (book != null)
            {
                var authors = db.GetAuthorsOfBook(book.ISBN);
                library.Books.Add(new BookModel(book, authors));
            }
        }

        library.Page = page;

        return library;
    }

    public IActionResult Library()
    {
        var library = MakeLibraryAtPage(db.GetAllBooks(), 1);

        return View("Library", library);
    }

    [Route("Home/Library/{page}")]
    public IActionResult Library([FromRoute] int page)
    {
        var library = MakeLibraryAtPage(db.GetAllBooks(), page);

        return View("Library", library);
    }

    [HttpPost]
    public IActionResult Search(string title)
    {
        var books = new List<Book> { db.GetBookByTitle(title) };
        var library = MakeLibraryAtPage(books, 1);

        return View("Search", library);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Borrowed(string ISBN)
    {
        var userId = Request.Cookies["id"];
        if (userId == null)
        {
            Response.Redirect("/Home/Login");
        }

        db.BorrowBook(ISBN, int.Parse(userId));
        Response.Redirect("/Home/Library");
        return new RedirectResult("/Home/Library");
    }
    
    [HttpPost]
    public IActionResult ReturnBook(string ISBN)
    {
        var userId = Request.Cookies["id"];
        if (userId == null)
        {
            Response.Redirect("/Home/Login");
        }
        
        db.ReturnBook(ISBN, int.Parse(userId));
        Response.Redirect("/Home/Home");
        return new RedirectResult("/Home/Home");
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}