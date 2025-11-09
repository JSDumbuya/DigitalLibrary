namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs;
using System.Diagnostics;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IUserService _userService;
    public LibraryService(ILibraryRepository libraryRepository, IUserService userService)
    {
        _libraryRepository = libraryRepository;
        _userService = userService;
    }

    public async Task<LibraryReadDTO> AddLibraryAsync(LibraryCreateDTO libraryCreateDTO, int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException($"User with id {userId} does not exist.");

        var existingLibrary = await _libraryRepository.GetLibraryByUserIdAsync(userId);
        if (existingLibrary != null) throw new InvalidOperationException("This user already has a library.");

        var toLibrary = MapperLibraryCreateDtoToLibrary(libraryCreateDTO, userId);
        var newLibrary = await _libraryRepository.AddAsync(toLibrary);
        
        return MapperLibraryToReadDTO(newLibrary);
    }

    public async Task<LibraryReadDTO?> GetLibraryByUserIdAsync(int userId)
    {
        var library = await _libraryRepository.GetLibraryByUserIdAsync(userId);
        if (library == null) throw new KeyNotFoundException($"The library for user with id {userId} does not exist.");

        return MapperLibraryToReadDTO(library);
    }

    public async Task<bool> DeleteLibraryAsync(int userId)
    {
        var result = await _libraryRepository.DeleteAsync(userId);
        if (!result) throw new KeyNotFoundException($"The library for user with id {userId} was not found or could not be deleted.");
        return result;
    }

    public async Task<bool> UpdateLibraryAsync(LibraryUpdateDTO libraryUpdateDTO, int userId)
    {
        var existingLibrary = await _libraryRepository.GetLibraryByUserIdAsync(userId);
        if (existingLibrary == null) throw new KeyNotFoundException($"The library for user with id {userId} does not exist.");

        var toLibrary = MapperLibraryUpdateDtoToLibrary(libraryUpdateDTO, userId, existingLibrary.Id);
        var updatedLibrary = await _libraryRepository.UpdateAsync(toLibrary, userId);
        if (!updatedLibrary) throw new InvalidOperationException($"Failed to update the library for user with id {userId}.");

        return updatedLibrary;
    }

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