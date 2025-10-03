namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
public interface IBookService
{
    Task<Book> AddBookAsync(Book book);
    Task<bool> UpdateBookAsync(Book book, int libraryId);
    Task<bool> DeleteBookAsync(int id, int libraryId);
    Task<Book?> GetBookByIdAsync(int id, int libraryId);
    Task<List<Book>> GetBooksAsync(
        BookStatus? status,
        BookGenre? genre,
        StarRating? rating,
        int libraryId);
    Task<List<Book>> GetBooksByStatusAsync(BookStatus status, int libraryId);
    Task<List<Book>> GetBooksByGenreAsync(BookGenre genre, int libraryId);
    Task<List<Book>> GetBooksByRatingAsync(StarRating rating, int libraryId);
    Task<List<Book>> GetBooksByLibraryIdAsync(int libraryId);
}