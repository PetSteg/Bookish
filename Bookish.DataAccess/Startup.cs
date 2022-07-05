using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Bookish.DataAccess;

public class Startup
{
    private string AppSettingsLocation = @"C:\Work\Training\Bookish\Bookish.DataAccess\appsettings.json";

    public IDbConnection ConfigureServices()
    {
        var AppSettings = new ConfigurationBuilder().AddJsonFile(AppSettingsLocation).Build();

        var connectionString = AppSettings.GetSection("ConnectionStrings")["SqlConnection"];
        IDbConnection db = new SqlConnection(connectionString);
        return db;
    }
}