using Bookish.DataAccess.Models;

namespace Bookish.Web.Models;

public class BorrowModel
{
    public int UserId { get; set; }
    
    public string ISBN { get; set; }

    public BorrowModel(int userId, string isbn)
    {
        this.ISBN = isbn;
        this.UserId = userId;
    }

    public BorrowModel()
    {
        
    }
}