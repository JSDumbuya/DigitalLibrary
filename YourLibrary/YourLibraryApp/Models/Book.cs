public class Book
{
    public int Id { get; set; }
    public int LibraryId { get; set; }
    public required string BookTitle { get; set; }
    public required string Author { get; set; }
    public BookStatus BookStatus { get; set; }
    public string? Review { get; set; }
    public StarRating? StarRating { get; set; }
    public BookGenre? Genre { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateFinished { get; set; }
}