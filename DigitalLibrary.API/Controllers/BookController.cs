using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;

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

    private Book MapperCreateDtoToBook(BookCreateDTO bCreateDto, int libraryId)
    {
        return new Book
        {
            LibraryId = libraryId,
            BookTitle = bCreateDto.BookTitle,
            Author = bCreateDto.Author,
            BookStatus = bCreateDto.BookStatus,
            Review = bCreateDto.Review,
            StarRating = bCreateDto.StarRating,
            Genre = bCreateDto.Genre,
            DateStarted = bCreateDto.DateStarted,
            DateFinished = bCreateDto.DateFinished

        };
    }

    private Book MapperUpdateDtoToBook(BookUpdateDTO bUpdateDto, int id, int libraryId)
    {
        return new Book
        {
            Id = id,
            LibraryId = libraryId,
            BookTitle = bUpdateDto.BookTitle,
            Author = bUpdateDto.Author,
            BookStatus = bUpdateDto.BookStatus,
            Review = bUpdateDto.Review,
            StarRating = bUpdateDto.StarRating,
            Genre = bUpdateDto.Genre,
            DateStarted = bUpdateDto.DateStarted,
            DateFinished = bUpdateDto.DateFinished,
        };
    }
}