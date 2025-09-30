using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

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
    public async Task<ActionResult<Book?>> GetBook(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null) return NotFound();
        return Ok(book);
    }
}