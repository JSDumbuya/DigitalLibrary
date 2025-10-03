namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Data;
using DigitalLibrary.API.Models;
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Task<Book> AddBookAsync(Book book)
    {
        return _bookRepository.AddAsync(book);
    }

    public Task<bool> DeleteBookAsync(int id, int libraryId)
    {
        return _bookRepository.DeleteAsync(id, libraryId);
    }

    public Task<Book?> GetBookByIdAsync(int id, int libraryId)
    {
        return _bookRepository.GetByIdAsync(id, libraryId);
    }

    public Task<List<Book>> GetBooks(BookStatus? status, BookGenre? genre, StarRating? rating, int libraryId)
    {
        if (status.HasValue)
            return GetBooksByStatusAsync(status.Value, libraryId);
        else if (genre.HasValue)
            return GetBooksByGenreAsync(genre.Value, libraryId);
        else if (rating.HasValue)
            return GetBooksByRatingAsync(rating.Value, libraryId);
        else
            return GetBooksByLibraryIdAsync(libraryId); 
    }

    public Task<List<Book>> GetBooksByGenreAsync(BookGenre genre, int libraryId)
    {
        return _bookRepository.GetByGenreAsync(genre, libraryId);
    }

    public Task<List<Book>> GetBooksByLibraryIdAsync(int libraryId)
    {
        return _bookRepository.GetByLibraryIdAsync(libraryId);
    }

    public Task<List<Book>> GetBooksByRatingAsync(StarRating rating, int libraryId)
    {
        return _bookRepository.GetByRatingAsync(rating, libraryId);
    }

    public Task<List<Book>> GetBooksByStatusAsync(BookStatus status, int libraryId)
    {
        return _bookRepository.GetByStatusAsync(status, libraryId);
    }

    public Task<bool> UpdateBookAsync(Book book, int libraryId)
    {
        return _bookRepository.UpdateAsync(book, libraryId);
    }
}