using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Data Source=YLDatabase.db";
        BookRepository bookRepository = new BookRepository(connectionString);
        UserRepository userRepository = new UserRepository(connectionString);
        LibraryRepository libraryRepository = new LibraryRepository(connectionString);

        //What is dependency injection here?
        //Services also go here
        //var bookService = new BookService(bookRepository);
    }
}

/*Setup repositories and services with dependency injection*/
