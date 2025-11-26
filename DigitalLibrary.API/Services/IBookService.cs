namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
public interface IBookService
{
    Task<BookReadDTO> AddBookAsync(BookCreateDTO bookCreateDTO, int userId);
    Task<bool> UpdateBookAsync(BookUpdateDTO bookUpdateDTO, int bookId, int userId);
    Task<bool> DeleteBookAsync(int id, int userId);
    Task<BookReadDTO?> GetBookByIdAsync(int id, int userId);
    Task<List<BookReadDTO>> GetBooksAsync(int userId, BookStatus? status, BookGenre? genre, StarRating? rating);
    Task<List<Book>> GetBooksByStatusAsync(BookStatus status, int libraryId);
    Task<List<Book>> GetBooksByGenreAsync(BookGenre genre, int libraryId);
    Task<List<Book>> GetBooksByRatingAsync(StarRating rating, int libraryId);
    Task<List<Book>> GetBooksByLibraryIdAsync(int libraryId);
}