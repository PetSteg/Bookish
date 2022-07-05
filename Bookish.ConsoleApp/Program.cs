using System.Data;
using Bookish.DataAccess;
using Dapper;

namespace Bookish;

static class Program
{
    public static async Task Main(string[] args)
    {
        // builder.Services.AddSingleton<DapperContext>();
        // builder.Services.AddControllers();
        Console.WriteLine("start");
    
        var db = new Startup().ConfigureServices();
        var result = await db.QueryAsync("SELECT * FROM Book", null, commandType: CommandType.Text);
    }
}