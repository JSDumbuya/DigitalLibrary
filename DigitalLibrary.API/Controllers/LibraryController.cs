using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;
using DigitalLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Route("api/users/{userId:int}/library")]
public class LibraryController : ControllerBase
{
    private readonly LibraryService _libraryService;

    public LibraryController(LibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    [HttpGet]
    public async Task<ActionResult<LibraryReadDTO>> GetLibrary([FromRoute] int userId)
    {
        var library = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (library == null) return NotFound();
        var dto = MapperToReadDTO(library);
        return Ok(dto);

    }

    [HttpPost]
    public async Task<ActionResult<LibraryReadDTO>> CreateLibrary([FromRoute] int userId, [FromBody] LibraryCreateDTO libraryCreateDTO)
    {
        var toLibrary = MapperToLibraryCreate(libraryCreateDTO, userId);
        var newLibrary = await _libraryService.AddLibraryAsync(toLibrary);
        var toDto = MapperToReadDTO(newLibrary);
        return CreatedAtAction(nameof(GetLibrary), new { userId = newLibrary.UserId }, toDto);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLibrary([FromRoute] int userId)
    {
        var library = await _libraryService.DeleteLibraryAsync(userId);
        if (!library) return NotFound();
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateLibrary([FromRoute] int userId, [FromBody] LibraryUpdateDTO updateDTO)
    {
        var existingLibrary = await _libraryService.GetLibraryByUserIdAsync(userId);
        if (existingLibrary == null) return NotFound();

        var toLibrary = MapperToLibraryUpdate(updateDTO, userId, existingLibrary.Id);
        var updatedLibrary = await _libraryService.UpdateLibraryAsync(toLibrary, userId);
        if (!updatedLibrary) return NotFound();
        return NoContent();
    }


    //Mappers
    private LibraryReadDTO MapperToReadDTO(Library library)
    {
        return new LibraryReadDTO
        {
            Id = library.Id,
            LibraryDescription = library.LibraryDescription,
            LibraryName = library.LibraryName
        };
    }

    private Library MapperToLibraryCreate(LibraryCreateDTO libraryCreateDTO, int userId)
    {
        return new Library
        {
            UserId = userId,
            LibraryDescription = libraryCreateDTO.LibraryDescription,
            LibraryName = libraryCreateDTO.LibraryName
        };
    }

    private Library MapperToLibraryUpdate(LibraryUpdateDTO updateDTO, int userId, int id)
    {
        return new Library
        {
            Id = id,
            UserId = userId,
            LibraryName = updateDTO.LibraryName,
            LibraryDescription = updateDTO.LibraryDescription
        };
    }

}