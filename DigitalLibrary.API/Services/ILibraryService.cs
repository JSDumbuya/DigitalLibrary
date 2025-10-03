namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
public interface ILibraryService
{
    Task<Library> AddLibraryAsync(Library library);
    Task<bool> UpdateLibraryAsync(Library library, int userId);
    Task<bool> DeleteLibraryAsync(int id, int userId);
    Task<Library?> GetLibraryByIdAsync(int id, int userId);
    Task<Library?> GetLibraryByUserIdAsync(int userId);
}