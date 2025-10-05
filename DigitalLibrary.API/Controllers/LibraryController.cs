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
    public async Task<ActionResult<LibraryReadDTO>> CreateLibrary([FromRoute] int userId, [FromBody] LibraryCreateDTO lcreateDto)
    {
        var toLibrary = MapperToLibrary(lcreateDto, userId);
        var newLibrary = await _libraryService.AddLibraryAsync(toLibrary);
        var toDto = MapperToReadDTO(newLibrary);
        return CreatedAtAction(nameof(GetLibrary), new { userId = newLibrary.UserId }, toDto);
    }


    //Mappers
    public LibraryReadDTO MapperToReadDTO(Library library)
    {
        return new LibraryReadDTO
        {
            Id = library.Id,
            LibraryDescription = library.LibraryDescription,
            LibraryName = library.LibraryName
        };
    }

    public Library MapperToLibrary(LibraryCreateDTO lcreateDto, int userId)
    {
        return new Library
        {
            UserId = userId,
            LibraryDescription = lcreateDto.LibraryDescription,
            LibraryName = lcreateDto.LibraryName
        };
    }

}