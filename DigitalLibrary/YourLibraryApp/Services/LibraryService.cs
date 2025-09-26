namespace DigitalLibrary.Services;

using DigitalLibrary.Models;
using DigitalLibrary.Data;

public class LibraryService : ILibraryService
{
    private readonly ILibraryRepository _libraryRepository;
    public LibraryService(ILibraryRepository libraryRepository)
    {
        _libraryRepository = libraryRepository;
    }
    public Task AddLibraryAsync(Library library)
    {
        return _libraryRepository.AddAsync(library);
    }

    public Task DeleteLibraryAsync(int id)
    {
        return _libraryRepository.DeleteAsync(id);
    }

    public Task<Library?> GetLibraryByIdAsync(int id)
    {
        return _libraryRepository.GetLibraryByIdAsync(id);
    }

    public Task<Library?> GetLibraryByUserIdAsync(int userId)
    {
        return _libraryRepository.GetLibraryByUserIdAsync(userId);
    }

    public Task UpdateLibraryAsync(Library library)
    {
        return _libraryRepository.UpdateAsync(library);
    }
}