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

    public async Task<List<Author>> GetAuthorsOfBook(string ISBN)
    {
        if (ISBN.Length != 13)
        {
            throw new Exception("Wrong ISBN");
        }

        var sqlQuery =
            $"SELECT * FROM Author INNER JOIN Contributions ON Author.Id_author = Contributions.Id_author WHERE Contributions.Id_book = {ISBN}";
        var authors = await db.QueryAsync<Author>(sqlQuery, null, commandType: CommandType.Text);

        return authors.ToList();
    }

    public async Task<List<Book>> GetAllBooks()
    {
        var sqlQuery = "SELECT * FROM Book ORDER BY [Title]";
        var books = await db.QueryAsync<Book>(sqlQuery, null, commandType: CommandType.Text);

        return books.ToList();
    }

    public async void InsertBorrow()
    {
        var date = DateTime.Now.ToString();
        var sqlQuery = $"INSERT INTO Bookish.dbo.Borrow (Id_book, Id_user, DueDate) Values ('9780747532743', 201, '{date}')";
        db.Execute(sqlQuery);
    }

    public async void InsertUser(string name, string email, string password)
    {
        password = Hash(password);

        var sqlQuery = $"INSERT INTO Bookish.dbo.User (Name, Email, Password) VALUES ('{name}','{email}','{password}')";
        db.Execute(sqlQuery);
    }

    public string Hash(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetNonZeroBytes(salt);
        }

        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return hashedPassword;
    }
    public async Task<List<Borrow>> GetAllBorrows()
    {
        var sqlQuery = "SELECT * FROM Borrow";
        var books = await db.QueryAsync<Borrow>(sqlQuery, null, commandType: CommandType.Text);

        return books.ToList();
    }
}