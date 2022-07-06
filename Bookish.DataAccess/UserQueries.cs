using System.Data;
using Bookish.DataAccess.Models;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Bookish.DataAccess;

public class UserQueries
{
    private IDbConnection db;

    public UserQueries(IDbConnection database)
    {
        db = database;
    }

    public string Hash(string password)
    {
        var salt = new byte[] { 8, 0, 0, 8, 1, 3, 5 };

        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
    }

    public bool InsertUser(string name, string email, string password)
    {
        if (GetUserByEmail(email) != null)
        {
            return false;
        }

        var sqlQuery =
            $"INSERT INTO Bookish.dbo.Users (Name, Email, Password) VALUES ('{name}', '{email}','{Hash(password)}')";
        return db.Execute(sqlQuery) == 1;
    }

    public bool VerifyUser(string email, string password)
    {
        var sqlQuery = $"SELECT Password FROM Bookish.dbo.Users WHERE Email = '{email}'";
        var user = db.Query<User>(sqlQuery)?.First();

        if (user == null)
        {
            return false;
        }

        return user.Password == Hash(password);
    }

    public User? GetUserByEmail(string email)
    {
        var sqlQuery = $"SELECT * FROM Bookish.dbo.Users WHERE Email = '{email}'";
        var users = db.Query<User>(sqlQuery, null, commandType: CommandType.Text);

        return !users.Any() ? null : users.First();
    }

    public User? GetUserById(string id)
    {
        var sqlQuery = $"SELECT * FROM Bookish.dbo.Users WHERE Id_user = '{id}'";
        var user = db.Query<User>(sqlQuery, null, commandType: CommandType.Text);

        return user?.First();
    }
}