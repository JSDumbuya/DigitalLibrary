namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
public interface IBookRepository
{
    Task<Book> AddAsync(Book book);
    Task<bool> UpdateAsync(Book book, int libraryId);
    Task<bool> DeleteAsync(int id, int libraryId);
    Task<Book?> GetByIdAsync(int id, int libraryId);
    Task<List<Book>> GetByStatusAsync(BookStatus status, int libraryId);
    Task<List<Book>> GetByGenreAsync(BookGenre genre, int libraryId);
    Task<List<Book>> GetByRatingAsync(StarRating rating, int libraryId);
    Task<List<Book>> GetByLibraryIdAsync(int libraryId);
}