namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Common;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _libraryRepository;
    private readonly IUserService _userService;
    public LibraryService(ILibraryRepository libraryRepository, IUserService userService)
    {
        _libraryRepository = libraryRepository;
        _userService = userService;
    }

    public async Task<Result<LibraryReadDTO>> AddLibraryAsync(LibraryCreateDTO libraryCreateDTO, int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return Result<LibraryReadDTO>.Fail(ErrorType.UserNotFound, "The user does not exist.");

        var existingLibrary = await _libraryRepository.GetLibraryByUserIdAsync(userId);
        if (existingLibrary != null) return Result<LibraryReadDTO>.Fail(ErrorType.LibraryAlreadyExists, "The user already has a library");
    
        var toLibrary = MapperLibraryCreateDtoToLibrary(libraryCreateDTO, userId);
        var newLibrary = await _libraryRepository.AddAsync(toLibrary);
        
        return Result<LibraryReadDTO>.Success(MapperLibraryToReadDTO(newLibrary));
    }

    public async Task<Result<LibraryReadDTO>> GetLibraryByUserIdAsync(int userId)
    {
        var library = await _libraryRepository.GetLibraryByUserIdAsync(userId);
        if (library == null) return Result<LibraryReadDTO>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");

        return Result<LibraryReadDTO>.Success(MapperLibraryToReadDTO(library));
    }

    public async Task<Result<bool>> DeleteLibraryAsync(int userId)
    {
        var result = await _libraryRepository.DeleteAsync(userId);
        if (!result) return Result<bool>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");
        
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdateLibraryAsync(LibraryUpdateDTO libraryUpdateDTO, int userId)
    {
        var existingLibrary = await _libraryRepository.GetLibraryByUserIdAsync(userId);
        if (existingLibrary == null) return Result<bool>.Fail(ErrorType.LibraryNotFound, "The user does not have an associated library.");
        

        var toLibrary = MapperLibraryUpdateDtoToLibrary(libraryUpdateDTO, userId, existingLibrary.Id);
        var updatedLibrary = await _libraryRepository.UpdateAsync(toLibrary, userId);
        if (!updatedLibrary) return Result<bool>.Fail(ErrorType.LibraryNotFound, "Library update failed because the library was not found.");

        return Result<bool>.Success(true);
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