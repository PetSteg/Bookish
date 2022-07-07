using System.Data;
using Bookish.DataAccess.Models;
using Dapper;

namespace Bookish.DataAccess;

public class BookQueries
{
    private IDbConnection db;

    public BookQueries(IDbConnection database)
    {
        db = database;
    }

    public List<Book> GetAllBooks()
    {
        var sqlQuery = "SELECT * FROM Bookish.dbo.Book ORDER BY [Title]";

        var books = db.Query<Book>(sqlQuery, null, commandType: CommandType.Text)?.ToList();
        if (books == null)
        {
            books = new List<Book>();
        }

        return books;
    }

    public Book GetBookByTitle(string title)
    {
        var sqlQuery = $"SELECT * FROM Bookish.dbo.Book WHERE Title = '{title}'";
        var book =  db.Query<Book>(sqlQuery, null, commandType: CommandType.Text)?.ToList();
        
        if (book != null && book.Count() > 0)
        {
            return book.First();
        }

        return null;
    }

    public List<Author> GetAuthorsOfBook(string isbn)
    {
        if (isbn.Length == 13)
        {
            var sqlQuery =
                $"SELECT * FROM Bookish.dbo.Author INNER JOIN Bookish.dbo.Contributions ON Author.Id_author = Contributions.Id_author WHERE Contributions.Id_book = '{isbn}'";

            var authors = db.Query<Author>(sqlQuery, null, commandType: CommandType.Text)?.ToList();
            if (authors != null)
            {
                return authors;
            }
        }

        return new List<Author>();
    }

    private int? GetAuthorId(string name)
    {
        var sqlQuery = $"SELECT Id_author FROM Bookish.dbo.Author WHERE Name = '{name}'";
        var authorId = db.Query<int?>(sqlQuery, null, commandType: CommandType.Text).FirstOrDefault(null as int?);

        return authorId;
    }

    private bool InsertAuthor(string name)
    {
        var sqlQuery = $"INSERT INTO Bookish.dbo.Author (Name) VALUES ('{name}')";
        return db.Execute(sqlQuery) == 1;
    }

    private int? GetOrAttemptToInsertAuthorId(string name)
    {
        var authorId = GetAuthorId(name);
        if (authorId != null)
        {
            return authorId;
        }

        InsertAuthor(name);
        return GetAuthorId(name);
    }

    private void InsertContribution(string isbn, int idAuthor)
    {
        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Contributions (Id_book, Id_author) VALUES ('{isbn}', '{idAuthor}')";
        db.Execute(sqlQuery);
    }

    public void InsertBook(string isbn, string title, string category, string publishDate, string subtitle,
        string coverPhotoUrl, int availableCopies, List<string> authors)
    {
        if (GetBookByIsbn(isbn) != null || availableCopies <= 0)
        {
            return;
        }

        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Book (ISBN, Title, Category, Publish_date, Subtitle, Cover_photo_url, Available_copies) VALUES ('{isbn}', '{title}', '{category}','{publishDate}', '{subtitle}', '{coverPhotoUrl}', {availableCopies})";
        db.Execute(sqlQuery);

        foreach (var author in authors)
        {
            var authorId = GetOrAttemptToInsertAuthorId(author);
            InsertContribution(isbn, (int)authorId!);
        }
    }

    public void InsertBook(Book book, List<string> authors)
    {
        InsertBook(book.ISBN, book.Title, book.Category, book.Publish_date, book.Subtitle, book.Cover_photo_url,
            book.Available_copies, authors);
    }

    public void InsertBorrow(string isbn, int userId)
    {
        var dueDate = DateTime.Now.AddDays(14).ToString("dd-MM-yyyy");
        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Borrow (Id_book, Id_user, Due_date) VALUES ('{isbn}', {userId}, '{dueDate}')";
        db.Execute(sqlQuery);
    }

    public List<Borrow> GetAllBorrows()
    {
        var sqlQuery = "SELECT * FROM Bookish.dbo.Borrow";
        var books = db.Query<Borrow>(sqlQuery, null, commandType: CommandType.Text);

        return books.ToList();
    }

    private int GetAvailableCopies(string isbn)
    {
        var sqlQuery = $"SELECT Available_copies FROM Bookish.dbo.Book WHERE ISBN = '{isbn}'";
        var availableCopies = db.Query<int>(sqlQuery)?.First();
        
        return availableCopies ?? 0;
    }

    private Book GetBookByIsbn(string isbn)
    {
        var sqlQuery = $"SELECT * FROM Bookish.dbo.Book WHERE ISBN = '{isbn}'";
        var book = db.Query<Book>(sqlQuery).FirstOrDefault(null as Book);

        return book;
    }

    private bool UpdateAvailableCopies(string isbn, int availableCopies)
    {
        var updateCopiesQuery =
            $"UPDATE Bookish.dbo.Book SET Available_copies = {availableCopies} WHERE ISBN = '{isbn}'";
        return db.Execute(updateCopiesQuery) == 1;
    }

    public bool BorrowBook(string isbn, int userId)
    {
        var availableCopies = GetAvailableCopies(isbn);
        if (availableCopies <= 0)
        {
            return false;
        }

        InsertBorrow(isbn, userId);

        return UpdateAvailableCopies(isbn, availableCopies - 1);
    }

    private bool RemoveBorrow(string isbn, int userId)
    {
        var deleteBorrowQuery =
            $"DELETE TOP (1) FROM Bookish.dbo.Borrow WHERE Id_book = '{isbn}' AND Id_user = {userId}";
        return db.Execute(deleteBorrowQuery) == 1;
    }

    public void ReturnBook(string isbn, int userId)
    {
        RemoveBorrow(isbn, userId);
        var availableCopies = GetAvailableCopies(isbn);

        UpdateAvailableCopies(isbn, availableCopies + 1);
    }

    public string GetDueDateUserBook(int idUser, string isbn)
    {
        var sqlQuery = $"SELECT Due_date FROM Bookish.dbo.Borrow WHERE Id_book = '{isbn}' AND Id_user = {idUser}";
        var dueDate = db.Query<string>(sqlQuery, null, commandType: CommandType.Text).First();

        return dueDate;
    }

    public List<Book> GetBooksBorrowedByUser(int idUser)
    {
        var sqlQuery =
            $"SELECT * FROM Bookish.dbo.Book INNER JOIN Bookish.dbo.Borrow ON Bookish.dbo.Book.ISBN = Bookish.dbo.Borrow.Id_book WHERE Bookish.dbo.Borrow.Id_user = {idUser}";
        var books = db.Query<Book>(sqlQuery, null, commandType: CommandType.Text);

        return books.ToList();
    }
}