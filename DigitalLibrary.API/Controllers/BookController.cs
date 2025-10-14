using Microsoft.AspNetCore.Mvc;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;

namespace DigitalLibrary.API.Controllers;


/// <summary>
/// Manages operations related to books within a user's library.
/// </summary>
/// <remarks>
/// Each book belongs to a single library, and each user can have only one library.  
/// This controller allows clients to retrieve, create, update, and delete books,
/// as well as filter them by genre, rating, or status.
/// </remarks>
[ApiController]
[Route("api/users/{userId:int}/library/books")]
public class BookController : ControllerBase
{
    private readonly BookService _bookService;
    private readonly LibraryService _libraryService;

    public BookController(BookService bookService, LibraryService libraryService)
    {
        _bookService = bookService;
        _libraryService = libraryService;
    }

    /// <summary>
    /// Retrieves a specific book by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the book to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the library containing the book.</param>
    /// <returns>A <see cref="BookReadDTO"/> representing the requested book.</returns>
    /// <response code="200">Successfully retrieved the book.</response>
    /// <response code="404">The specified book or library was not found.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookReadDTO>> GetBookById([FromRoute] int id, [FromRoute] int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return NotFound();

        var book = await _bookService.GetBookByIdAsync(id, library.Id);
        if (book == null) return NotFound();

        var dto = MapperBookToReadDTO(book);
        return Ok(dto);
    }

    /// <summary>
    /// Retrieves all books in the user's library, with optional filters.
    /// </summary>
    /// <param name="genre">An optional filter by book genre.</param>
    /// <param name="userId">The ID of the user whose library is being accessed.</param>
    /// <param name="rating">An optional filter by book star rating.</param>
    /// <param name="status">An optional filter by book reading status.</param>
    /// <returns>A list of <see cref="BookReadDTO"/> objects representing the user's books.</returns>
    /// <response code="200">Successfully retrieved the list of books.</response>
    /// <response code="404">No books found for the specified filters or user.</response>
    [HttpGet]
    public async Task<ActionResult<List<BookReadDTO>>> GetBooks([FromQuery] BookGenre? genre, [FromRoute] int userId, [FromQuery] StarRating? rating, [FromQuery] BookStatus? status)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return NotFound();

        var books = await _bookService.GetBooksAsync(status, genre, rating, library.Id);
        if (books.Count == 0) return NotFound();

        var bookDTOs = books.Select(MapperBookToReadDTO).ToList();
        //200 - success
        return Ok(bookDTOs);
    }

    /// <summary>
    /// Creates a new book within the user's library.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the library where the book will be created.</param>
    /// <param name="bookCreateDTO">The data used to create the new book.</param>
    /// <returns>
    /// A <see cref="BookReadDTO"/> representing the newly created book.
    /// </returns>
    /// <response code="201">Book successfully created.</response>
    /// <response code="404">No library found for the specified user.</response>
    /// <response code="400">Invalid book data supplied.</response>
    [HttpPost]
    public async Task<ActionResult<BookReadDTO>> CreateBook([FromRoute] int userId, [FromBody] BookCreateDTO bookCreateDTO)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return NotFound();

        var toBook = MapperCreateDtoToBook(bookCreateDTO, library.Id);
        var createdBook = await _bookService.AddBookAsync(toBook);
        var toDTO = MapperBookToReadDTO(createdBook);
        //201 - succes
        return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id, userId }, toDTO);
    }

    /// <summary>
    /// Updates an existing book in the user's library.
    /// </summary>
    /// <param name="userId">The ID of the user who owns the library containing the book.</param>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="bookUpdateDTO">The updated book data.</param>
    /// <response code="204">Book successfully updated.</response>
    /// <response code="404">The specified book or library was not found.</response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook([FromRoute] int userId, [FromRoute] int id, [FromBody] BookUpdateDTO bookUpdateDTO)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return NotFound();

        var toBook = MapperUpdateDtoToBook(bookUpdateDTO, id, library.Id);
        var updatedBook = await _bookService.UpdateBookAsync(toBook, library.Id);
        if (!updatedBook) return NotFound();
        //204 - succes
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a specific book from the user's library.
    /// </summary>
    /// <param name="id">The unique identifier of the book to delete.</param>
    /// <param name="userId">The ID of the user who owns the library containing the book.</param>
    /// <response code="204">Book successfully deleted.</response>
    /// <response code="404">The specified book or library was not found.</response>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook([FromRoute] int id, [FromRoute] int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return NotFound();

        var deletedBook = await _bookService.DeleteBookAsync(id, library.Id);
        if (!deletedBook) return NotFound();
        return NoContent();
    }

    //Mappers
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