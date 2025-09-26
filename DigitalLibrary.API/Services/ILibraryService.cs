namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
public interface ILibraryService
{
    Task AddLibraryAsync(Library library);
    Task UpdateLibraryAsync(Library library);
    Task DeleteLibraryAsync(int id);
    Task<Library?> GetLibraryByIdAsync(int id);
    Task<Library?> GetLibraryByUserIdAsync(int userId);
}