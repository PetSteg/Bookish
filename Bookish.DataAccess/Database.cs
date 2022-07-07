using System.Data;
using Bookish.DataAccess.Models;

namespace Bookish.DataAccess;

public class Database
{
    private static IDbConnection db;
    private static UserQueries userQueries;
    private static BookQueries bookQueries;

    static Database()
    {
        db = new Startup().ConfigureServices();
        userQueries = new UserQueries(db);
        bookQueries = new BookQueries(db);
    }

    public bool InsertUser(string name, string email, string password)
    {
        return userQueries.InsertUser(name, email, password);
    }

    public bool VerifyUser(string email, string password)
    {
        return userQueries.VerifyUser(email, password);
    }

    public User GetUserByEmail(string email)
    {
        return userQueries.GetUserByEmail(email);
    }

    public User GetUserById(string? userId)
    {
        return userQueries.GetUserById(userId);
    }

    public List<Book> GetBooksBorrowedByUser(int userId)
    {
        return bookQueries.GetBooksBorrowedByUser(userId);
    }

    public string GetDueDateUserBook(int userId, string ISBN)
    {
        return bookQueries.GetDueDateUserBook(userId, ISBN);
    }

    public List<Book> GetAllBooks()
    {
        return bookQueries.GetAllBooks();
    }

    public List<Author> GetAuthorsOfBook(string ISBN)
    {
        return bookQueries.GetAuthorsOfBook(ISBN);
    }

    public void BorrowBook(string isbn, int userId)
    {
        bookQueries.BorrowBook(isbn, userId);
    }

    public void InsertBook(string isbn, string title, string category, string publishDate, string subtitle,
        string coverPhotoUrl, int availableCopies, List<string> authors)
    {
        bookQueries.InsertBook(isbn, title, category, publishDate, subtitle, coverPhotoUrl, availableCopies, authors);
    }

    public void InsertBook(Book book, List<string> authors)
    {
        bookQueries.InsertBook(book, authors);
    }
}