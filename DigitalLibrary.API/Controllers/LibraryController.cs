
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using DigitalLibrary.API.Common;
using Microsoft.AspNetCore.Authorization;

namespace DigitalLibrary.API.Controllers;

/// <summary>
/// Manages operations related to a user's library.
/// </summary>
/// <remarks>
/// Each user can only have one library.  
/// This controller allows clients to create, retrieve, update, and delete a user's library.
/// </remarks>
[ApiController]
[Route("api/library")]
//[Route("api/users/{userId:int}/library")]
[Authorize]
public class LibraryController : ControllerBase
{
    private readonly ILibraryService _libraryService;

    public LibraryController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    /// <summary>
    /// Retrieves the library associated with a specific user.
    /// </summary>
    /// <returns>
    /// A <see cref="LibraryReadDTO"/> object representing the user's library.
    /// </returns>
    /// <response code="200">Successfully retrieved the user's library.</response>
    /// <response code="404">No library found for the specified user.</response>
    /// <response code="401">Invalid or missing JWT token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    public async Task<ActionResult<LibraryReadDTO>> GetLibrary()
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _libraryService.GetLibraryByUserIdAsync(userId);
        if(!result.IsSuccess)
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
    /// Creates a new library for a specific user.
    /// </summary>
    /// <param name="libraryCreateDTO">The data used to create the new library.</param>
    /// <returns>
    /// The newly created <see cref="LibraryReadDTO"/> object.
    /// </returns>
    /// <response code="201">Successfully created a new library for the user.</response>
    /// <response code="404">The specified user was not found.</response>
    /// <response code="409">The library already exists</response>
    /// <response code="401">Invalid or missing JWT token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    public async Task<ActionResult<LibraryReadDTO>> CreateLibrary([FromBody] LibraryCreateDTO libraryCreateDTO)
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _libraryService.AddLibraryAsync(libraryCreateDTO, userId);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.UserNotFound => NotFound(result.Message),
                ErrorType.LibraryAlreadyExists => Conflict(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return CreatedAtAction(nameof(GetLibrary), new { userId }, result.Value);
    }
    
    /// <summary>
    /// Deletes the library associated with a specific user.
    /// </summary>
    /// <response code="204">Successfully deleted the user's library.</response>
    /// <response code="404">No library found for the specified user.</response>
    /// <response code="401">Invalid or missing JWT token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpDelete]
    public async Task<IActionResult> DeleteLibrary()
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _libraryService.DeleteLibraryAsync(userId);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.LibraryNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return NoContent();
    }


    /// <summary>
    /// Updates the library associated with a specific user.
    /// </summary>
    /// <param name="libraryUpdateDTO">The updated library information.</param>
    /// <response code="204">Successfully updated the user's library.</response>
    /// <response code="404">No library found for the specified user.</response>
    /// <response code="401">Invalid or missing JWT token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPut]
    public async Task<IActionResult> UpdateLibrary([FromBody] LibraryUpdateDTO libraryUpdateDTO)
    {
        if (!UserClaimsHelper.TryGetUserId(this, out int userId)) return Unauthorized();

        var result = await _libraryService.UpdateLibraryAsync(libraryUpdateDTO, userId);
        if (!result.IsSuccess)
        {
            return result.Type switch
            {
                ErrorType.LibraryNotFound => NotFound(result.Message),
                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        return NoContent();
    }

}