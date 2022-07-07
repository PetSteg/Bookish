using Bookish.DataAccess.Models;

namespace Bookish.Web.Models;

public class BookModel
{
    public Book Book { get; set; }
    public string AuthorsString { get; set; }
    public List<Author> Authors { get; set; }
    public string DueDate { get; set; }
    public bool? Error { get; set; }

    public BookModel()
    {
    }

    public BookModel(Book book, List<Author> authors)
    {
        Book = book;
        Authors = authors;
    }

    public BookModel(Book book, string dueDate)
    {
        Book = book;
        DueDate = dueDate;
    }
}