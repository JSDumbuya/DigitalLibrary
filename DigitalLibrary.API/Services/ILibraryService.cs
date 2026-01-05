namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Common;

public interface ILibraryService
{
    Task<Result<LibraryReadDTO>> AddLibraryAsync(LibraryCreateDTO libraryCreateDTO, int userId);
    Task<Result<bool>> UpdateLibraryAsync(LibraryUpdateDTO libraryUpdateDTO, int userId);
    Task<Result<bool>> DeleteLibraryAsync(int userId);
    Task<Result<LibraryReadDTO>> GetLibraryByUserIdAsync(int userId);
}