namespace DigitalLibrary.API.Models;
public class Library
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string LibraryName { get; set; }
    public string? LibraryDescription { get; set; }
}