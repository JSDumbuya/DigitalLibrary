namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
using Microsoft.EntityFrameworkCore;

public class LibraryRepository : ILibraryRepository
{
    private readonly DigitalLibraryContext _context;
    public LibraryRepository(DigitalLibraryContext context)
    {
        _context = context;
    }

    public async Task<Library> AddAsync(Library library)
    {
        _context.Libraries.Add(library);
        await _context.SaveChangesAsync();
        return library;
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var library = await _context.Libraries.FirstOrDefaultAsync(library => library.Id == id && library.UserId == userId);
        if (library == null) return false;

        _context.Libraries.Remove(library);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Library?> GetLibraryByIdAsync(int id, int userId)
    {
        return await _context.Libraries.FirstOrDefaultAsync(library => library.Id == id && library.UserId == userId);
    }

    public async Task<Library?> GetLibraryByUserIdAsync(int userId)
    {
        return await _context.Libraries.FirstOrDefaultAsync(library => library.UserId == userId);
    }

    public async Task<bool> UpdateAsync(Library library, int userId)
    {
        var existingLibrary = await _context.Libraries.FirstOrDefaultAsync(l => l.Id == library.Id && l.UserId == userId);
        if (existingLibrary == null) return false;

        if (library.LibraryDescription != null)
            existingLibrary.LibraryDescription = library.LibraryDescription;
        existingLibrary.LibraryName = library.LibraryName;

        await _context.SaveChangesAsync();
        return true;
        
    }
}