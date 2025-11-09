namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.DTOs;
public interface ILibraryService
{
    Task<LibraryReadDTO> AddLibraryAsync(LibraryCreateDTO libraryCreateDTO, int userId);
    Task<bool> UpdateLibraryAsync(LibraryUpdateDTO libraryUpdateDTO, int userId);
    Task<bool> DeleteLibraryAsync(int userId);
    Task<LibraryReadDTO?> GetLibraryByUserIdAsync(int userId);
}