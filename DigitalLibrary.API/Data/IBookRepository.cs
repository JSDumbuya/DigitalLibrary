namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
public interface IBookRepository
{
    Task AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> GetByStatusAsync(BookStatus status);
    Task<List<Book>> GetByGenreAsync(BookGenre genre);
    Task<List<Book>> GetByRatingAsync(StarRating rating);
    Task<List<Book>> GetByLibraryIdAsync(int libraryId);
}