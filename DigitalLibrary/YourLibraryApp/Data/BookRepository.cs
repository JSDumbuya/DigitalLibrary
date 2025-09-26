namespace DigitalLibrary.Data;

using Microsoft.Data.Sqlite;
using DigitalLibrary.Models;

public class BookRepository : IBookRepository
{
    private readonly string _connectionString;
    public BookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddAsync(Book book)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
        INSERT INTO Books
        (LibraryId, BookTitle, Author, BookStatus, Review, StarRating, Genre, DateAdded, DateFinished)
        VALUES ($LibraryId, $BookTitle, $Author, $BookStatus, $Review, $StarRating, $Genre, $DateAdded, $DateFinished);
        SELECT last_insert_rowid();";

        command.Parameters.AddWithValue("$LibraryId", book.LibraryId);
        command.Parameters.AddWithValue("$BookTitle", book.BookTitle);
        command.Parameters.AddWithValue("$Author", book.Author);
        command.Parameters.AddWithValue("$BookStatus", (int)book.BookStatus);
        command.Parameters.AddWithValue("$Review", (object?)book.Review ?? DBNull.Value);
        command.Parameters.AddWithValue("$StarRating", (object?)book.StarRating ?? DBNull.Value);
        command.Parameters.AddWithValue("$Genre", (object?)book.Genre ?? DBNull.Value);
        command.Parameters.AddWithValue("$DateAdded", book.DateAdded);
        command.Parameters.AddWithValue("$DateFinished", (object?)book.DateFinished ?? DBNull.Value);

        var result = await command.ExecuteScalarAsync();
        if (result == null)
        {
            Console.WriteLine("Something went wrong. Book was not added");
            return;
        }
        book.Id = Convert.ToInt32(result);
        Console.WriteLine($"A new Library was added with the following Id: {book.Id}");
    }


    public async Task DeleteAsync(int id)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Books WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        if (affectedRows == 0)
        {
            Console.WriteLine("No book was deleted. Book may not exist");
        }

    }

    public async Task<List<Book>> GetByGenreAsync(BookGenre genre)
    {
        return await GetBooksByFilterAsync("Genre", (int)genre);
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Books WHERE Id = $Id;";
        command.Parameters.AddWithValue("$Id", id);

        using SqliteDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapRawToBook(reader);
        }

        return null;
    }

    public async Task<List<Book>> GetByLibraryIdAsync(int libraryId)
    {
        return await GetBooksByFilterAsync("LibraryId", libraryId);
    }

    public async Task<List<Book>> GetByRatingAsync(StarRating rating)
    {
        return await GetBooksByFilterAsync("StarRating", (int)rating);
    }

    public async Task<List<Book>> GetByStatusAsync(BookStatus status)
    {
        return await GetBooksByFilterAsync("BookStatus", (int)status);
    }

    public async Task UpdateAsync(Book book)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"UPDATE Books SET
            LibraryId = $LibraryId,
            BookTitle = $BookTitle,
            Author = $Author,
            BookStatus = $BookStatus,
            Review = $Review,
            StarRating = $StarRating,
            Genre = $Genre,
            DateAdded = $DateAdded,
            DateFinished = $DateFinished
        WHERE Id = $Id;";
        command.Parameters.AddWithValue("$LibraryId", book.LibraryId);
        command.Parameters.AddWithValue("$BookTitle", book.BookTitle);
        command.Parameters.AddWithValue("$Author", book.Author);
        command.Parameters.AddWithValue("$BookStatus", (int)book.BookStatus);
        command.Parameters.AddWithValue("$Review", (object?)book.Review ?? DBNull.Value);
        command.Parameters.AddWithValue("$StarRating", (object?)book.StarRating ?? DBNull.Value);
        command.Parameters.AddWithValue("$Genre", (object?)book.Genre ?? DBNull.Value);
        command.Parameters.AddWithValue("$DateAdded", book.DateAdded);
        command.Parameters.AddWithValue("$DateFinished", (object?)book.DateFinished ?? DBNull.Value);
        command.Parameters.AddWithValue("$Id", book.Id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        if (affectedRows == 0)
        {
            Console.WriteLine("No book was updated. Book may not exist");
        }
    }

    private async Task<List<Book>> GetBooksByFilterAsync(string columnName, object value)
    {
        List<Book> books = new List<Book>();

        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM Books WHERE {columnName} = $value;";
        command.Parameters.AddWithValue("$value", value);

        using SqliteDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            books.Add(MapRawToBook(reader));
        }

        return books;
    }

    /// <summary>
    /// This method converts the current row of the reader into a Book object
    /// </summary>
    /// <param name="reader"></param>
    /// <returns>A Book object populated with data from the current row.</returns>
    private Book MapRawToBook(SqliteDataReader reader)
    {
        return new Book
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            LibraryId = reader.GetInt32(reader.GetOrdinal("LibraryId")),
            BookTitle = reader.GetString(reader.GetOrdinal("BookTitle")),
            Author = reader.GetString(reader.GetOrdinal("Author")),
            BookStatus = (BookStatus)reader.GetInt32(reader.GetOrdinal("BookStatus")),
            Review = reader.IsDBNull(reader.GetOrdinal("Review")) ? null : reader.GetString(reader.GetOrdinal("Review")),
            StarRating = reader.IsDBNull(reader.GetOrdinal("StarRating")) ? null : (StarRating)reader.GetInt32(reader.GetOrdinal("StarRating")),
            Genre = reader.IsDBNull(reader.GetOrdinal("Genre")) ? null : (BookGenre)reader.GetInt32(reader.GetOrdinal("Genre")),
            DateAdded = reader.GetDateTime(reader.GetOrdinal("DateAdded")),
            DateFinished = reader.IsDBNull(reader.GetOrdinal("DateFinished")) ? null : reader.GetDateTime(reader.GetOrdinal("DateFinished"))
        };
    }
}