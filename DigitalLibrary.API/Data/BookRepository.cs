namespace DigitalLibrary.API.Data;

using Microsoft.Data.Sqlite;
using DigitalLibrary.API.Models;
using Microsoft.EntityFrameworkCore;

public class BookRepository : IBookRepository
{
    private readonly DigitalLibraryContext _context;
    public BookRepository(DigitalLibraryContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            //Add message
            return;
        }
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

    }

    public async Task<List<Book>> GetByGenreAsync(BookGenre genre)
    {
        return await _context.Books
            .Where(book => book.Genre == genre)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    public async Task<List<Book>> GetByLibraryIdAsync(int libraryId)
    {
        return await _context.Books
            .Where(book => book.LibraryId == libraryId)
            .ToListAsync();
    }

    public async Task<List<Book>> GetByRatingAsync(StarRating rating)
    {
        return await _context.Books
            .Where(book => book.StarRating == rating)
            .ToListAsync();
    }

    public async Task<List<Book>> GetByStatusAsync(BookStatus status)
    {
        return await _context.Books
            .Where(book => book.BookStatus == status)
            .ToListAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        var existingBook = await _context.Books.FindAsync(book.Id);
        if (existingBook == null)
        {
            //add message
            return;
        }
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
    }
}