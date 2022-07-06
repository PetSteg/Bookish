using Bookish.DataAccess;

namespace Bookish;

static class Program
{
    public static async Task Main(string[] args)
    {
        var db = new Database();
        // Console.WriteLine("Books:");
        // var books = await db.GetAllBooks();
        // foreach (var book in books)
        // {
        //     Console.WriteLine(book.Title);
        //     var authors = await db.GetAuthorsOfBook(book.ISBN);
        //     foreach (var author in authors)
        //     {
        //         Console.WriteLine(author.Name);
        //     }
        // }
        
        // foreach (var book in db.GetBooksBorrowedByUser(208))
        // {
        //     Console.WriteLine(book.Title);
        //     Console.WriteLine(book.ISBN);
        //     var authors = db.GetAuthorsOfBook(book.ISBN);
        //     foreach (var author in authors)
        //     {
        //         Console.WriteLine(author.Name);
        //     }
        //     Console.WriteLine();
        // }
        
        /*db.InsertBorrow();

        var borrows = await db.GetAllBorrows();
        foreach (var borrow in borrows)
        {
            Console.WriteLine(borrow.Id_user + " " + borrow.Due_date);
        }*/
        // db.InsertBook("123456789abi", "C# for dummies", "Programming", "05-07-2022", "Yes", 2, new List<string>(){"Stefan", "Peter"});
        /*var aux = db.GetAuthorId("Jon");
        Console.WriteLine(aux);*/
        
        /*db.InsertUser("Stefan2","Stefan@ceva.com","1234");

        Console.WriteLine(db.VerifyUser("Stefan@ceva.com", "12345"));
        Console.WriteLine(db.VerifyUser("Stefan@ceva.com", "1234"));*/

        /*Console.WriteLine(db.BorrowBook("123456789abcd", 208));
        */
        // db.ReturnBook("123456789abcd", 208);

        // var books = db.GetBooksBorrowedByUser(208);
        // foreach (var book in books)
        // {
        //     Console.WriteLine(book.Title);
        // }
    }
}