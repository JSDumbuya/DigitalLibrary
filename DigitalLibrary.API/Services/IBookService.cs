namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Common;
public interface IBookService
{
    Task<Result<BookReadDTO>> AddBookAsync(BookCreateDTO bookCreateDTO, int userId);
    Task<Result<bool>> UpdateBookAsync(BookUpdateDTO bookUpdateDTO, int bookId, int userId);
    Task<Result<bool>> DeleteBookAsync(int id, int userId);
    Task<Result<BookReadDTO>> GetBookByIdAsync(int id, int userId);
    Task<Result<List<BookReadDTO>>> GetBooksAsync(int userId, BookStatus? status, BookGenre? genre, StarRating? rating);
    Task<List<Book>> GetBooksByStatusAsync(BookStatus status, int libraryId);
    Task<List<Book>> GetBooksByGenreAsync(BookGenre genre, int libraryId);
    Task<List<Book>> GetBooksByRatingAsync(StarRating rating, int libraryId);
    Task<List<Book>> GetBooksByLibraryIdAsync(int libraryId);
}