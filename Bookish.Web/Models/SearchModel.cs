namespace Bookish.Web.Models;

public class SearchModel
{
    public BookModel bookModel;

    public SearchModel(BookModel bookModel)
    {
        this.bookModel = bookModel;
    }
}