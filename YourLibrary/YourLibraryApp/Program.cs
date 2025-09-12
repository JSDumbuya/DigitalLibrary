

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=YLDatabase.db";

        DatabaseInitializer.InitializeDatabase(connectionString);

        BookRepository bookRepository = new BookRepository(connectionString);
        UserRepository userRepository = new UserRepository(connectionString);
        LibraryRepository libraryRepository = new LibraryRepository(connectionString);

        BookService bookService = new BookService(bookRepository);
        UserService userService = new UserService(userRepository);
        LibraryService libraryService = new LibraryService(libraryRepository);

    }
}

