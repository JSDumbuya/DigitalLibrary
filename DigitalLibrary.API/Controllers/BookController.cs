using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;

[ApiController]
[Route("api/book")]
public class BookController : ControllerBase
{
    private readonly BookService _bookService;

    public BookController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{libraryId:int}/{id:int}")]
    public async Task<ActionResult<BookReadDTO>> GetBookById([FromRoute] int id, [FromRoute] int libraryId)
    {
        var book = await _bookService.GetBookByIdAsync(id, libraryId);
        if (book == null) return NotFound();
        var dto = MapperBookToReadDTO(book);
        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<List<BookReadDTO>>> GetBooks([FromQuery] BookGenre? genre, [FromQuery] int libraryId, [FromQuery] StarRating? rating, [FromQuery] BookStatus? status)
    {
        var books = await _bookService.GetBooksAsync(status, genre, rating, libraryId);
        if (books.Count == 0) return NotFound();
        var bookDTOs = books.Select(MapperBookToReadDTO).ToList();
        //200 - success
        return Ok(bookDTOs);
    }

    [HttpPost("{libraryId:int}")]
    public async Task<ActionResult<BookReadDTO>> CreateBook([FromRoute] int libraryId, [FromBody] BookCreateDTO bCreateDto)
    {
        var toBook = MapperCreateDtoToBook(bCreateDto, libraryId);
        var createdBook = await _bookService.AddBookAsync(toBook);
        var toDTO = MapperBookToReadDTO(createdBook);
        //201 - succes
        return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id, libraryId }, toDTO);
    }

    [HttpPut("{libraryId:int}/{id:int}")]
    public async Task<IActionResult> UpdateBook([FromRoute] int libraryId, [FromRoute] int id, [FromBody] BookUpdateDTO bUpdateDto)
    {
        var toBook = MapperUpdateDtoToBook(bUpdateDto, id, libraryId);
        var updatedBook = await _bookService.UpdateBookAsync(toBook, libraryId);
        if (!updatedBook) return NotFound();
        //204 - succes
        return NoContent();
    }

    [HttpDelete("{libraryId:int}/{id:int}")]
    public async Task<IActionResult> DeleteBook([FromRoute] int id, [FromRoute] int libraryId)
    {
        var deletedBook = await _bookService.DeleteBookAsync(id, libraryId);
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