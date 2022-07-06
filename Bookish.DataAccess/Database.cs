using System.Data;
using System.Security.Cryptography;
using Bookish.DataAccess.Models;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.VisualBasic;

namespace Bookish.DataAccess;

public class Database
{
    private static IDbConnection db;

    static Database()
    {
        db = new Startup().ConfigureServices();
    }

    public List<Author> GetAuthorsOfBook(string ISBN)
    {
        if (ISBN.Length != 13)
        {
            throw new Exception("Wrong ISBN");
        }

        var sqlQuery =
            $"SELECT * FROM Bookish.dbo.Author INNER JOIN Bookish.dbo.Contributions ON Author.Id_author = Contributions.Id_author WHERE Contributions.Id_book = '{ISBN}'";
        var authors = db.Query<Author>(sqlQuery, null, commandType: CommandType.Text);

        return authors.ToList();
    }

    public string GetDueDateUserBook(int idUser, string ISBN)
    {
        var sqlQuery = $"SELECT Due_date FROM Bookish.dbo.Borrow WHERE Id_book = '{ISBN}' AND Id_user = {idUser}";
        var dueDate = db.Query<string>(sqlQuery, null, commandType: CommandType.Text).First();

        return dueDate;
    }

    public List<Book> GetAllBooks()
    {
        var sqlQuery = "SELECT * FROM Bookish.dbo.Book ORDER BY [Title]";
        var books = db.Query<Book>(sqlQuery, null, commandType: CommandType.Text);

        return books.ToList();
    }

    public async void InsertBorrow(string ISBN, int id_user)
    {
        var dueDate = DateTime.Now.AddDays(14).ToString("dd-MM-yyyy");
        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Borrow (Id_book, Id_user, Due_date) VALUES ('{ISBN}', {id_user}, '{dueDate}')";
        db.Execute(sqlQuery);
    }

    public async void InsertUser(string name, string email, string password)
    {
        password = Hash(password);

        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Users (Name, Email, Password) VALUES ('{name}', '{email}','{password}')";
        db.Execute(sqlQuery);
    }

    public async void InsertBook(string ISBN, string Title, string Category, string Publish_date, string Subtitle,
        int Available_copies, List<string> authors)
    {
        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Book (ISBN, Title, Category, Publish_date, Subtitle, Available_copies) VALUES ('{ISBN}', '{Title}', " +
            $"'{Category}','{Publish_date}', '{Subtitle}','{Available_copies}')";
        db.Execute(sqlQuery);

        foreach (var author in authors)
        {
            var authorId = GetAuthorId(author);
            InsertContribution(ISBN, authorId);
        }
    }

    public string Hash(string password)
    {
        var salt = new byte[] { 0, 1, 2, 3 };

        var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return hashedPassword;
    }

    public int GetAuthorId(string name)
    {
        var sqlQuery = $"SELECT Id_author FROM Bookish.dbo.Author WHERE Name = '{name}'";
        var authors = db.Query<int>(sqlQuery, null, commandType: CommandType.Text).ToList();

        if (authors.Count > 0)
        {
            return authors[0];
        }

        var insertQuery = $"INSERT INTO Bookish.dbo.Author (Name) VALUES ('{name}')";
        db.Execute(insertQuery);

        sqlQuery = $"SELECT Id_author FROM Bookish.dbo.Author WHERE Name = '{name}'";
        authors = db.Query<int>(sqlQuery, null, commandType: CommandType.Text).ToList();

        return authors[0];
    }

    public async Task<List<Borrow>> GetAllBorrows()
    {
        var sqlQuery = "SELECT * FROM Bookish.dbo.Borrow";
        var books = await db.QueryAsync<Borrow>(sqlQuery, null, commandType: CommandType.Text);

        return books.ToList();
    }

    public async void InsertContribution(string ISBN, int id_author)
    {
        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Contributions (Id_book, Id_author) VALUES ('{ISBN}', '{id_author}')";
        db.Execute(sqlQuery);
    }

    public bool VerifyUser(string email, string password)
    {
        var sqlQuery = $"SELECT Password FROM Bookish.dbo.Users WHERE Email = '{email}'";
        try
        {
            var users = db.Query<User>(sqlQuery).ToList();

            if (users.Count < 0)
            {
                return false;
            }

            return users[0].Password == Hash(password);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool BorrowBook(string ISBN, int id_user)
    {
        var availableCopiesQuery = $"SELECT Available_copies FROM Bookish.dbo.Book WHERE ISBN = '{ISBN}'";
        var availableCopies = db.Query<int>(availableCopiesQuery).First();
        if (availableCopies <= 0)
        {
            return false;
        }

        InsertBorrow(ISBN, id_user);
        availableCopies--;
        var updateCopiesQuery =
            $"UPDATE Bookish.dbo.Book SET Available_copies = {availableCopies} WHERE ISBN = '{ISBN}'";
        db.Execute(updateCopiesQuery);
        return true;
    }

    public void ReturnBook(string ISBN, int id_user)
    {
        var deleteBorrowQuery =
            $"DELETE TOP (1) FROM Bookish.dbo.Borrow WHERE Id_book = '{ISBN}' AND Id_user = {id_user}";
        db.Execute(deleteBorrowQuery);

        var availableCopiesQuery = $"SELECT Available_copies FROM Bookish.dbo.Book WHERE ISBN = '{ISBN}'";
        var availableCopies = db.Query<int>(availableCopiesQuery).First();

        availableCopies++;
        var updateCopiesQuery =
            $"UPDATE Bookish.dbo.Book SET Available_copies = {availableCopies} WHERE ISBN = '{ISBN}'";
        db.Execute(updateCopiesQuery);
    }

    public List<Book> GetBooksBorrowedByUser(int idUser)
    {
        var sqlQuery =
            $"SELECT * FROM Bookish.dbo.Book INNER JOIN Bookish.dbo.Borrow ON Bookish.dbo.Book.ISBN = Bookish.dbo.Borrow.Id_book WHERE Bookish.dbo.Borrow.Id_user = {idUser}";
        var books = db.Query<Book>(sqlQuery, null, commandType: CommandType.Text);

        return books.ToList();
    }

    public User GetUserByEmail(string email)
    {
        var sqlQuery = $"SELECT * FROM Bookish.dbo.Users WHERE Email = '{email}'";
        var user = db.Query<User>(sqlQuery, null, commandType: CommandType.Text);

        return user.First();
    }

    public User GetUserById(string id)
    {
        var sqlQuery = $"SELECT * FROM Bookish.dbo.Users WHERE Id_user = '{id}'";
        var user = db.Query<User>(sqlQuery, null, commandType: CommandType.Text);

        return user.First();
    }
}