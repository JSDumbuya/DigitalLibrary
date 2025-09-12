
public interface IBookService
{
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int id);
    Task<Book?> GetBookByIdAsync(int id);
    Task<List<Book>> GetBooksByStatusAsync(BookStatus status);
    Task<List<Book>> GetBooksByGenreAsync(BookGenre genre);
    Task<List<Book>> GetBooksByRatingAsync(StarRating rating);
    Task<List<Book>> GetBooksByLibraryIdAsync(int libraryId);
}