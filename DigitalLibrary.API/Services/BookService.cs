namespace DigitalLibrary.API.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ILibraryService _libraryService;
    public BookService(IBookRepository bookRepository, ILibraryService libraryService)
    {
        _bookRepository = bookRepository;
        _libraryService = libraryService;
    }

    public async Task<BookReadDTO> AddBookAsync(BookCreateDTO bookCreateDTO, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) throw new KeyNotFoundException($"The library for user with id {userId} does not exist.");

        var toBook = MapperCreateDtoToBook(bookCreateDTO, library.Id);
        var createdBook = await _bookRepository.AddAsync(toBook);

        return MapperBookToReadDTO(createdBook);
    }

    public async Task<bool> DeleteBookAsync(int bookId, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) throw new KeyNotFoundException($"The library does not exist.");

        var result = await _bookRepository.DeleteAsync(bookId, library.Id);
        if (!result) throw new KeyNotFoundException($"The book with id {bookId} could not be found or could not be deleted.");

        return result;
    }

    public async Task<BookReadDTO?> GetBookByIdAsync(int bookId, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) throw new KeyNotFoundException($"The library does not exist.");

        var book = await _bookRepository.GetByIdAsync(bookId, library.Id);
        if (book == null) throw new KeyNotFoundException($"The book with id {bookId} could not be found.");

        return MapperBookToReadDTO(book);
    }

    public async Task<List<BookReadDTO>> GetBooksAsync(int userId, BookStatus? status, BookGenre? genre, StarRating? rating)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) throw new KeyNotFoundException($"The library does not exist.");

        List<Book> books;

        if (status.HasValue)
            books = await GetBooksByStatusAsync(status.Value, library.Id);
        else if (genre.HasValue)
            books = await GetBooksByGenreAsync(genre.Value, library.Id);
        else if (rating.HasValue)
            books = await GetBooksByRatingAsync(rating.Value, library.Id);
        else
            books = await GetBooksByLibraryIdAsync(library.Id); 

        if ( books == null || books.Count == 0) throw new KeyNotFoundException($"No books were found using this filter.");
        var bookDTOs = books.Select(MapperBookToReadDTO).ToList();

        return bookDTOs;
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

    public async Task<bool> UpdateBookAsync(BookUpdateDTO bookUpdateDTO, int bookId, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) throw new KeyNotFoundException($"The library for user with id {userId} does not exist.");

        var toBook = MapperUpdateDtoToBook(bookUpdateDTO, bookId, library.Id);
        var updatedBook = await _bookRepository.UpdateAsync(toBook, library.Id);
        if (!updatedBook) throw new InvalidOperationException($"Failed to update the book with id {toBook.Id}.");

        return updatedBook;
    }

    private BookReadDTO MapperBookToReadDTO(Book book)
    {
        return new BookReadDTO
        {
            Id = book.Id,
            BookTitle = book.BookTitle,
            Author = book.Author,
            BookStatus = book.BookStatus,
            Review = book.Review,
            StarRating = book.StarRating,
            Genre = book.Genre,
            DateStarted = book.DateStarted,
            DateFinished = book.DateFinished
        };
    }

    private Book MapperCreateDtoToBook(BookCreateDTO bookCreateDto, int libraryId)
    {
        return new Book
        {
            LibraryId = libraryId,
            BookTitle = bookCreateDto.BookTitle,
            Author = bookCreateDto.Author,
            BookStatus = bookCreateDto.BookStatus,
            Review = bookCreateDto.Review,
            StarRating = bookCreateDto.StarRating,
            Genre = bookCreateDto.Genre,
            DateStarted = bookCreateDto.DateStarted,
            DateFinished = bookCreateDto.DateFinished

        };
    }

    private Book MapperUpdateDtoToBook(BookUpdateDTO bookUpdateDto, int id, int libraryId)
    {
        return new Book
        {
            Id = id,
            LibraryId = libraryId,
            BookTitle = bookUpdateDto.BookTitle,
            Author = bookUpdateDto.Author,
            BookStatus = bookUpdateDto.BookStatus,
            Review = bookUpdateDto.Review,
            StarRating = bookUpdateDto.StarRating,
            Genre = bookUpdateDto.Genre,
            DateStarted = bookUpdateDto.DateStarted,
            DateFinished = bookUpdateDto.DateFinished,
        };
    }
}