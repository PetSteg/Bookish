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

    public async Task<List<Book>> GetAllBooks()
    {
        var result = (List<Book>)await db.QueryAsync("SELECT * FROM Book", null, commandType: CommandType.Text);
        return result;
    }
}