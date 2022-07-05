namespace Bookish.DataAccess.Models;

public class Borrow
{
    public string Id_book { get; set; }
    public int Id_user { get; set; }
    public DateTime DueDate { get; set; }
}