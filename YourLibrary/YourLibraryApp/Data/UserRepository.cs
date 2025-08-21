using Microsoft.Data.Sqlite;

public class UserRepository: IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddAsync(User user)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Users
            (UserName)
            VALUES ($UserName);
            SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$UserName", user.UserName);

        var result = await command.ExecuteScalarAsync();
        if (result == null)
        {
            Console.WriteLine("Something went wrong. User was not added");
            return;
        }
        user.Id = Convert.ToInt32(result);
        Console.WriteLine($"A new User was added with the following Id: {user.Id}");
    }

    public async Task DeleteAsync(int id)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Users WHERE Id = $Id";
        command.Parameters.AddWithValue("$Id", id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        if (affectedRows == 0)
        {
            Console.WriteLine("No user was deleted. User may not exist");
        }
        
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Users WHERE Id = $Id";
        command.Parameters.AddWithValue("$Id", id);

        using SqliteDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapRawToUser(reader);
        }

        return null;
    }

    private User MapRawToUser(SqliteDataReader reader)
    {
        return new User
        {
            Id = reader.GetInt32(reader.GetOrdinal("Id")),
            UserName = reader.GetString(reader.GetOrdinal("UserName"))
        };
    }

    public async Task UpdateAsync(User user)
    {
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Users SET
                UserName = $UserName
            WHERE Id = $Id";
        command.Parameters.AddWithValue("$UserName", user.UserName);
        command.Parameters.AddWithValue("$Id", user.Id);

        int affectedRows = await command.ExecuteNonQueryAsync();
        if (affectedRows == 0)
        {
            Console.WriteLine("No user was updated. User may not exist");
        }
    }
}