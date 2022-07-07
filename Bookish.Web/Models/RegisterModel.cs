namespace Bookish.Web.Models;

public class RegisterModel
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool? Error { get; set; }
}