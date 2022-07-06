using Bookish.DataAccess.Models;

namespace Bookish.Web.Models;

public class BookModel
{
    public Book Book { get; set; }
    public List<Author> Authors { get; set; }
    public string DueDate { get; set; }

    private BookModel()
    {
        if (Book.Cover_photo_url.Length < 5)
        {
            Book.Cover_photo_url =
                "https://sportshub.cbsistatic.com/i/2022/06/10/91e49e5d-41c3-4252-a649-fbf540595907/english-harry-potter-7-epub-9781781100264.jpg?auto=webp&width=1200&height=1800&crop=0.667:1,smart";
        }
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