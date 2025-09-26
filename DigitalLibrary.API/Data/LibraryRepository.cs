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

    public async Task AddAsync(Library library)
    {
        _context.Libraries.Add(library);
        await _context.SaveChangesAsync();
        //Add confirmation message
    }

    public async Task DeleteAsync(int id)
    {
        var library = await _context.Libraries.FindAsync(id);
        if (library == null)
        {
            //Add message
            return;
        }
        _context.Libraries.Remove(library);
        await _context.SaveChangesAsync();
    }

    public async Task<Library?> GetLibraryByIdAsync(int id)
    {
        return await _context.Libraries.FindAsync(id);
    }

    public async Task<Library?> GetLibraryByUserIdAsync(int userId)
    {
        return await _context.Libraries.FirstOrDefaultAsync(library => library.UserId == userId);
    }

    public async Task UpdateAsync(Library library)
    {
        var existingLibrary = await _context.Libraries.FindAsync(library.Id);
        if (existingLibrary == null)
        {
            //Add message
            return;
        }

        existingLibrary.LibraryDescription = library.LibraryDescription;
        existingLibrary.LibraryName = library.LibraryName;

        await _context.SaveChangesAsync();
        
    }
}