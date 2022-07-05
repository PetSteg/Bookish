namespace Bookish.DataAccess.Models;

public class Book
{
    private string ISBN { get; set; }
    private string Title { get; set; }
    private string Category { get; set; }
    private DateOnly Publish_date { get; set; }
    private string Subtitle { get; set; }
    private string Cover_photo_url { get; set; }
    private int Available_copies { get; set; }
}