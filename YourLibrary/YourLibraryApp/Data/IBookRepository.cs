
using System.Reflection.Metadata;

public interface IBookRepository
{
    void addBook();
    void updateBook();
    void deleteBook();
    void getBookByID();
}