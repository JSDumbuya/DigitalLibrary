namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
public interface ILibraryRepository
{
    Task<Library> AddAsync(Library library);
    Task<bool> UpdateAsync(Library library, int userId);
    Task<bool> DeleteAsync(int id, int userId);
    Task<Library?> GetLibraryByIdAsync(int id, int userId);
    Task<Library?> GetLibraryByUserIdAsync(int userId);
}