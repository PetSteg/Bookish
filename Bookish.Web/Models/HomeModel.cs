using Bookish.DataAccess.Models;

namespace Bookish.Web.Models;

public class HomeModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<BookModel> Books { get; set; }
}