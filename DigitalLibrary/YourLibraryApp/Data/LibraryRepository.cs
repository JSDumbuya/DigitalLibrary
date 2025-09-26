namespace DigitalLibrary.Data;

using DigitalLibrary.Models;
using Microsoft.Data.Sqlite;

public class LibraryRepository : ILibraryRepository
{
    private readonly string _connectionString;
    public LibraryRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddAsync(Library library)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Libraries
            (UserId, LibraryName, LibraryDescription)
            VALUES ($UserId, $LibraryName, $LibraryDescription);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$UserId", library.UserId);
        command.Parameters.AddWithValue("$LibraryName", library.LibraryName);
        command.Parameters.AddWithValue("$LibraryDescription", (object?)library.LibraryDescription ?? DBNull.Value);

        var result = await command.ExecuteScalarAsync();
        if (result == null)
        {
            Console.WriteLine("Something went wrong. Library was not added");
            return;
        }
        library.Id = Convert.ToInt32(result);
        Console.WriteLine($"A new Library was added with the following Id: {library.Id}");     
    }

    public async Task DeleteAsync(int id)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM Libraries WHERE Id = $Id";
        command.Parameters.AddWithValue("$Id", id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        if (affectedRows == 0)
        {
            Console.WriteLine("No library was deleted. Library may not exist");
        }
    }

    public async Task<Library?> GetLibraryByIdAsync(int id)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM Libraries WHERE Id = $Id";
        command.Parameters.AddWithValue("$Id", id);

        using SqliteDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapRawToLibrary(reader);
        }

        return null;
    }

    public async Task<Library?> GetLibraryByUserIdAsync(int userId)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Libraries WHERE UserId = $UserId";
        command.Parameters.AddWithValue("$UserId", userId);

        using SqliteDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return MapRawToLibrary(reader);
        }

        return null;
    }

    private Library MapRawToLibrary(SqliteDataReader reader)
    {
        return new Library
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
            LibraryName = reader.GetString(reader.GetOrdinal("LibraryName")),
            LibraryDescription = reader.IsDBNull(reader.GetOrdinal("LibraryDescription")) ? null : reader.GetString(reader.GetOrdinal("LibraryDescription"))
        };
    }

    public async Task UpdateAsync(Library library)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
        UPDATE Libraries SET
            UserId = $UserId,
            LibraryName = $LibraryName,
            LibraryDescription = $LibraryDescription
        WHERE Id = $Id;";

        command.Parameters.AddWithValue("$UserId", library.UserId);
        command.Parameters.AddWithValue("$LibraryName", library.LibraryName);
        command.Parameters.AddWithValue("$LibraryDescription", (object?)library.LibraryDescription ?? DBNull.Value);

        int affectedRows = await command.ExecuteNonQueryAsync();
        if (affectedRows == 0)
        {
            Console.WriteLine("No user was updated. User may not exist");
        }
    }
}