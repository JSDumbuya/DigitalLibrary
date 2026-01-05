namespace DigitalLibrary.API.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Common;
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ILibraryService _libraryService;
    public BookService(IBookRepository bookRepository, ILibraryService libraryService)
    {
        _bookRepository = bookRepository;
        _libraryService = libraryService;
    }

    public async Task<Result<BookReadDTO>> AddBookAsync(BookCreateDTO bookCreateDTO, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return Result<BookReadDTO>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");

        var toBook = MapperCreateDtoToBook(bookCreateDTO, library.Value.Id);
        var createdBook = await _bookRepository.AddAsync(toBook);

        return Result<BookReadDTO>.Success(MapperBookToReadDTO(createdBook));
    }

    public async Task<Result<bool>> DeleteBookAsync(int bookId, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return Result<bool>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");

        var deleted = await _bookRepository.DeleteAsync(bookId, library.Value.Id);
        if (!deleted) return Result<bool>.Fail(ErrorType.BookNotFound, "The specified book does not exist in the library.");

        return Result<bool>.Success(true);
    }

    public async Task<Result<BookReadDTO>> GetBookByIdAsync(int bookId, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return Result<BookReadDTO>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");

        var book = await _bookRepository.GetByIdAsync(bookId, library.Value.Id);
        if (book == null) return Result<BookReadDTO>.Fail(ErrorType.BookNotFound, "The specified book does not exist in the library.");

        return Result<BookReadDTO>.Success(MapperBookToReadDTO(book));
    }

    public async Task<Result<List<BookReadDTO>>> GetBooksAsync(int userId, BookStatus? status, BookGenre? genre, StarRating? rating)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return Result<List<BookReadDTO>>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");

        List<Book> books;

        if (status.HasValue)
            books = await GetBooksByStatusAsync(status.Value, library.Value.Id);
        else if (genre.HasValue)
            books = await GetBooksByGenreAsync(genre.Value, library.Value.Id);
        else if (rating.HasValue)
            books = await GetBooksByRatingAsync(rating.Value, library.Value.Id);
        else
            books = await GetBooksByLibraryIdAsync(library.Value.Id); 

        var bookDTOs = (books ?? new List<Book>()).Select(MapperBookToReadDTO).ToList();

        return Result<List<BookReadDTO>>.Success(bookDTOs);
    }

    public async Task<Result<bool>> UpdateBookAsync(BookUpdateDTO bookUpdateDTO, int bookId, int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return Result<bool>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");

        var toBook = MapperUpdateDtoToBook(bookUpdateDTO, bookId, library.Value.Id);
        var updatedBook = await _bookRepository.UpdateAsync(toBook, library.Value.Id);
        if (!updatedBook) return Result<bool>.Fail(ErrorType.BookNotFound, "The specified book does not exist in the library.");

        return Result<bool>.Success(true);
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