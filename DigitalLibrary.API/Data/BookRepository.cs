namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private readonly DigitalLibraryContext _context;
    public BookRepository(DigitalLibraryContext context)
    {
        _context = context;
    }

    public async Task<Book> AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        //For use in HTTP response
        return book;
    }

    public async Task<bool> DeleteAsync(int id, int libraryId)
    {
        var book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id && book.LibraryId == libraryId);
        if (book == null) return false;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Book>> GetByGenreAsync(BookGenre genre, int libraryId)
    {
        return await _context.Books
            .Where(book => book.Genre == genre && book.LibraryId == libraryId)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id, int libraryId)
    {
        return await _context.Books.FirstOrDefaultAsync(book => book.Id == id && book.LibraryId == libraryId);
    }

    public async Task<List<Book>> GetByLibraryIdAsync(int libraryId)
    {
        return await _context.Books
            .Where(book => book.LibraryId == libraryId)
            .ToListAsync();
    }

    public async Task<List<Book>> GetByRatingAsync(StarRating rating, int libraryId)
    {
        return await _context.Books
            .Where(book => book.StarRating == rating && book.LibraryId == libraryId)
            .ToListAsync();
    }

    public async Task<List<Book>> GetByStatusAsync(BookStatus status, int libraryId)
    {
        return await _context.Books
            .Where(book => book.BookStatus == status && book.LibraryId == libraryId)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Book book, int libraryId)
    {
        var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == book.Id && b.LibraryId == libraryId);
        if (existingBook == null) return false;

        existingBook.BookTitle = book.BookTitle;
        existingBook.Author = book.Author;
        existingBook.BookStatus = book.BookStatus;
        if (book.Review != null)
            existingBook.Review = book.Review;
        if (book.StarRating != null)
            existingBook.StarRating = book.StarRating;
        if (book.Genre != null)
            existingBook.Genre = book.Genre;
        if (book.DateFinished != null)
            existingBook.DateFinished = book.DateFinished;

        await _context.SaveChangesAsync();
        return true;
    }
}