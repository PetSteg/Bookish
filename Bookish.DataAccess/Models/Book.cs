namespace Bookish.DataAccess.Models;

public class Book
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Category { get; set; }
    public string Publish_date { get; set; }
    public string Subtitle { get; set; }
    public string Cover_photo_url { get; set; }
    public int Available_copies { get; set; }
}