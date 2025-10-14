using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

/// <summary>
/// Manages operations related to a user's library.
/// </summary>
/// <remarks>
/// Each user can only have one library.  
/// This controller allows clients to create, retrieve, update, and delete a user's library.
/// </remarks>
[ApiController]
[Route("api/users/{userId:int}/library")]
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
    /// <param name="userId">The ID of the user whose library is being retrieved.</param>
    /// <returns>
    /// A <see cref="LibraryReadDTO"/> object representing the user's library.
    /// </returns>
    /// <response code="200">Successfully retrieved the user's library.</response>
    /// <response code="404">No library found for the specified user.</response>
    [HttpGet]
    public async Task<ActionResult<LibraryReadDTO>> GetLibrary([FromRoute] int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return NotFound();

        var dto = MapperLibraryToReadDTO(library);
        return Ok(dto);

    }

    /// <summary>
    /// Creates a new library for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user for whom the library will be created.</param>
    /// <param name="libraryCreateDTO">The data used to create the new library.</param>
    /// <returns>
    /// The newly created <see cref="LibraryReadDTO"/> object.
    /// </returns>
    /// <response code="201">Successfully created a new library for the user.</response>
    /// <response code="400">The provided library data is invalid.</response>
    [HttpPost]
    public async Task<ActionResult<LibraryReadDTO>> CreateLibrary([FromRoute] int userId, [FromBody] LibraryCreateDTO libraryCreateDTO)
    {
        var toLibrary = MapperLibraryCreateDtoToLibrary(libraryCreateDTO, userId);
        var newLibrary = await _libraryService.AddLibraryAsync(toLibrary);
        var toDto = MapperLibraryToReadDTO(newLibrary);
        return CreatedAtAction(nameof(GetLibrary), new { userId = newLibrary.UserId }, toDto);
    }
    
    /// <summary>
    /// Deletes the library associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose library will be deleted.</param>
    /// <response code="204">Successfully deleted the user's library.</response>
    /// <response code="404">No library found for the specified user.</response>
    [HttpDelete]
    public async Task<IActionResult> DeleteLibrary([FromRoute] int userId)
    {
        var library = await _libraryService.DeleteLibraryAsync(userId);
        if (!library) return NotFound();
        return NoContent();
    }


    /// <summary>
    /// Updates the library associated with a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose library is being updated.</param>
    /// <param name="updateDTO">The updated library information.</param>
    /// <response code="204">Successfully updated the user's library.</response>
    /// <response code="404">No library found for the specified user.</response>
    [HttpPut]
    public async Task<IActionResult> UpdateLibrary([FromRoute] int userId, [FromBody] LibraryUpdateDTO updateDTO)
    {
        var existingLibrary = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (existingLibrary == null) return NotFound();

        var toLibrary = MapperLibraryUpdateDtoToLibrary(updateDTO, userId, existingLibrary.Id);
        var updatedLibrary = await _libraryService.UpdateLibraryAsync(toLibrary, userId);
        if (!updatedLibrary) return NotFound();
        return NoContent();
    }


    //Mappers
    private LibraryReadDTO MapperLibraryToReadDTO(Library library)
    {
        return new LibraryReadDTO
        {
            Id = library.Id,
            LibraryDescription = library.LibraryDescription,
            LibraryName = library.LibraryName
        };
    }

    private Library MapperLibraryCreateDtoToLibrary(LibraryCreateDTO libraryCreateDTO, int userId)
    {
        return new Library
        {
            UserId = userId,
            LibraryDescription = libraryCreateDTO.LibraryDescription,
            LibraryName = libraryCreateDTO.LibraryName
        };
    }

    private Library MapperLibraryUpdateDtoToLibrary(LibraryUpdateDTO libraryUpdateDTO, int userId, int id)
    {
        return new Library
        {
            Id = id,
            UserId = userId,
            LibraryName = libraryUpdateDTO.LibraryName,
            LibraryDescription = libraryUpdateDTO.LibraryDescription
        };
    }

}