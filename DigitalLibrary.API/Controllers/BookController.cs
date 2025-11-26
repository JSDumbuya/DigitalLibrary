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
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Retrieves a specific book by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the book to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the library containing the book.</param>
    /// <returns>A <see cref="BookReadDTO"/> representing the requested book.</returns>
    /// <response code="200">Book successfully retrieved.</response>
    /// <response code="404">The specified book or library was not found.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookReadDTO>> GetBookById([FromRoute] int id, [FromRoute] int userId)
    {
        try
        {
            var bookReadDTO = await _bookService.GetBookByIdAsync(id, userId);
            return Ok(bookReadDTO);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all books in the user's library, with optional filters.
    /// </summary>
    /// <param name="genre">An optional filter by book genre.</param>
    /// <param name="userId">The ID of the user whose library is being accessed.</param>
    /// <param name="rating">An optional filter by book star rating.</param>
    /// <param name="status">An optional filter by book reading status.</param>
    /// <returns>A list of <see cref="BookReadDTO"/> objects representing the user's books.</returns>
    /// <response code="200">Books successfully retrieved.</response>
    /// <response code="404">No books found for the specified filters or user.</response>
    [HttpGet]
    public async Task<ActionResult<List<BookReadDTO>>> GetBooks([FromQuery] BookGenre? genre, [FromRoute] int userId, [FromQuery] StarRating? rating, [FromQuery] BookStatus? status)
    {
        try
        {
            var bookDTOs = await _bookService.GetBooksAsync(userId, status, genre, rating);
            return Ok(bookDTOs);
        }
        catch (KeyNotFoundException ex)
        {
            
            return NotFound(ex.Message);
        }
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
    /// <response code="400">Invalid data supplied.</response>
    [HttpPost]
    public async Task<ActionResult<BookReadDTO>> CreateBook([FromRoute] int userId, [FromBody] BookCreateDTO bookCreateDTO)
    {
        try
        {
            var createdBookReadDTO = await _bookService.AddBookAsync(bookCreateDTO, userId);
            //201 - succes
            return CreatedAtAction(nameof(GetBookById), new { id = createdBookReadDTO.Id, userId }, createdBookReadDTO);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
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
        try
        {
            await _bookService.UpdateBookAsync(bookUpdateDTO, id, userId);
            //204 - succes
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
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
        try
        {
            await _bookService.DeleteBookAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}