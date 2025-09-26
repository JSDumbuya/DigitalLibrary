namespace DigitalLibrary.Services;

using DigitalLibrary.Data;
using DigitalLibrary.Models;
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Task AddBookAsync(Book book)
    {
        return _bookRepository.AddAsync(book);
    }

    public Task DeleteBookAsync(int id)
    {
        return _bookRepository.DeleteAsync(id);
    }

    public Task<Book?> GetBookByIdAsync(int id)
    {
        return _bookRepository.GetByIdAsync(id);
    }

    public Task<List<Book>> GetBooksByGenreAsync(BookGenre genre)
    {
        return _bookRepository.GetByGenreAsync(genre);
    }

    public Task<List<Book>> GetBooksByLibraryIdAsync(int libraryId)
    {
        return _bookRepository.GetByLibraryIdAsync(libraryId);
    }

    public Task<List<Book>> GetBooksByRatingAsync(StarRating rating)
    {
        return _bookRepository.GetByRatingAsync(rating);
    }

    public Task<List<Book>> GetBooksByStatusAsync(BookStatus status)
    {
        return _bookRepository.GetByStatusAsync(status);
    }

    public Task UpdateBookAsync(Book book)
    {
        return _bookRepository.UpdateAsync(book);
    }
}