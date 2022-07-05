using Bookish.DataAccess;

namespace Bookish;

static class Program
{
    public static async Task Main(string[] args)
    {
        var db = new Database();
        Console.WriteLine("Books:");
        var books = await db.GetAllBooks();
        foreach (var book in books)
        {
            Console.WriteLine(book.Title);
            var authors = await db.GetAuthorsOfBook(book.ISBN);
            foreach (var author in authors)
            {
                Console.WriteLine(author.Name);
            }
        }
    }
}