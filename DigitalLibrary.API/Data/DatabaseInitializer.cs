namespace DigitalLibrary.Data;

using Microsoft.Data.Sqlite;

public class DatabaseInitializer
{
    public static void InitializeDatabase(string connectionString)
    {
        using SqliteConnection connection = new SqliteConnection(connectionString);
        connection.Open();

        string usersTable = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserName TEXT NOT NULL
        );";

        string libraryTable = @"
        CREATE TABLE IF NOT EXISTS Libraries (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            LibraryName TEXT NOT NULL,
            LibraryDescription TEXT,
            FOREIGN KEY (UserId) REFERENCES Users (Id),
        );";

        string booksTable = @"
        CREATE TABLE IF NOT EXISTS Books (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            LibraryId INTEGER NOT NULL,
            BookTitle TEXT NOT NULL,
            Author TEXT NOT NULL,
            BookStatus INTEGER NOT NULL,
            Review TEXT,
            StarRating INTEGER,
            Genre INTEGER,
            DateAdded DATETIME NOT NULL,
            DateFinished DATETIME,
            FOREIGN KEY (LibraryId) REFERENCES Libraries (Id)
        );";

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = usersTable;
        command.ExecuteNonQuery();

        command.CommandText = libraryTable;
        command.ExecuteNonQuery();

        command.CommandText = booksTable;
        command.ExecuteNonQuery();
    }
}