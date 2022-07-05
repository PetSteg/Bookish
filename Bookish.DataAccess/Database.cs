using System.Data;
using Bookish.DataAccess.Models;
using Dapper;

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
}