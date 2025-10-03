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

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookReadDTO>> GetBookById(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);

        if (book == null) return NotFound();

        var dto = MapperReadDTO(book);

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<List<BookReadDTO>>> GetBooks(
        [FromQuery] BookGenre? genre,
        [FromQuery] int? libraryId,
        [FromQuery] StarRating? rating,
        [FromQuery] BookStatus? status
    )
    {
        var books = await _bookService
    }

    //Mappers
    private BookReadDTO MapperReadDTO(Book book)
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
            DateAdded = book.DateAdded,
            DateFinished = book.DateFinished
        };
    }
}