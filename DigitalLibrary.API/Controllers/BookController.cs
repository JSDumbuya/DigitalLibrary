using Microsoft.AspNetCore.Mvc;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;
using DigitalLibrary.API.Common;
using Microsoft.AspNetCore.Authorization;

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
//Ensures that every endpoint requires auth - all endpoints are private.
//Could be placed above individual methods - mixed public/private endpoints.
[Authorize]
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
    /// <returns>A <see cref="BookReadDTO"/> representing the requested book.</returns>
    /// <response code="200">Book successfully retrieved.</response>
    /// <response code="404">The specified book or library was not found.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookReadDTO>> GetBookById([FromRoute] int id)
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _bookService.GetBookByIdAsync(id, userId);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.LibraryNotFound => NotFound(result.Message),
                ErrorType.BookNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Retrieves all books in the user's library, with optional filters.
    /// </summary>
    /// <param name="genre">An optional filter by book genre.</param>
    /// <param name="rating">An optional filter by book star rating.</param>
    /// <param name="status">An optional filter by book reading status.</param>
    /// <returns>A list of <see cref="BookReadDTO"/> objects representing the user's books.</returns>
    /// <response code="200">Books successfully retrieved.</response>
    /// <response code="404">No books found for the specified filters or user.</response>
    [HttpGet]
    public async Task<ActionResult<List<BookReadDTO>>> GetBooks([FromQuery] BookGenre? genre, [FromQuery] StarRating? rating, [FromQuery] BookStatus? status)
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _bookService.GetBooksAsync(userId, status, genre, rating);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.LibraryNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates a new book within the user's library.
    /// </summary>
    /// <param name="bookCreateDTO">The data used to create the new book.</param>
    /// <returns>
    /// A <see cref="BookReadDTO"/> representing the newly created book.
    /// </returns>
    /// <response code="201">Book successfully created.</response>
    /// <response code="404">No library found for the specified user.</response>
    /// <response code="400">Invalid data supplied.</response>
    [HttpPost]
    public async Task<ActionResult<BookReadDTO>> CreateBook([FromBody] BookCreateDTO bookCreateDTO)
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();
        
        var result = await _bookService.AddBookAsync(bookCreateDTO, userId);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.LibraryNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return CreatedAtAction(nameof(GetBookById), new { id = result.Value.Id, userId }, result.Value);
    }

    /// <summary>
    /// Updates an existing book in the user's library.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="bookUpdateDTO">The updated book data.</param>
    /// <response code="204">Book successfully updated.</response>
    /// <response code="404">The specified book or library was not found.</response>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookUpdateDTO bookUpdateDTO)
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _bookService.UpdateBookAsync(bookUpdateDTO, id, userId);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.LibraryNotFound => NotFound(result.Message),
                ErrorType.BookNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return NoContent();
    }
    
    /// <summary>
    /// Deletes a specific book from the user's library.
    /// </summary>
    /// <param name="id">The unique identifier of the book to delete.</param>
    /// <response code="204">Book successfully deleted.</response>
    /// <response code="404">The specified book or library was not found.</response>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook([FromRoute] int id)
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _bookService.DeleteBookAsync(id, userId);
         if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.LibraryNotFound => NotFound(result.Message),
                ErrorType.BookNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return NoContent();
    }
}