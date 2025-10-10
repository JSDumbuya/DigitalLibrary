namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.Data;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _libraryRepository;
    public LibraryService(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }
    public Task<Library> AddLibraryAsync(Library library)
    {
        return _libraryRepository.AddAsync(library);
    }

    public Task<bool> DeleteLibraryAsync(int userId)
    {
        return _libraryRepository.DeleteAsync(userId);
    }

    /*public Task<Library?> GetLibraryByIdAsync(int id, int userId)
    {
        return _libraryRepository.GetLibraryByIdAsync(id, userId);
    }*/

    public Task<Library?> GetLibraryByUserIdAsync(int userId)
    {
        return _libraryRepository.GetLibraryByUserIdAsync(userId);
    }

    public Task<bool> UpdateLibraryAsync(Library library, int userId)
    {
        return _libraryRepository.UpdateAsync(library, userId);
    }
}