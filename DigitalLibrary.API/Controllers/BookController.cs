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

    [HttpGet("{id}")]
    public async Task<ActionResult<BookReadDTO>> GetBook(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);

        if (book == null) return NotFound();

        var dto = MapToReadDTO(book);
        
        return Ok(dto);
    }

    private BookReadDTO MapToReadDTO(Book book)
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