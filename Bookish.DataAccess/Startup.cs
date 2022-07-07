using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Bookish.DataAccess;

public class Startup
{
    private const string AppSettingsLocation = @"C:\Work\Training\Bookish2\Bookish\Bookish.DataAccess\appsettings.json";

    public IDbConnection ConfigureServices()
    {
        var appSettings = new ConfigurationBuilder().AddJsonFile(AppSettingsLocation).Build();
        var connectionString = appSettings.GetSection("ConnectionStrings")["SqlConnection"];

        IDbConnection db = new SqlConnection(connectionString);
        db.Open();

        return db;
    }
}