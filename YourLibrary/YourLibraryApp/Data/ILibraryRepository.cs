
public interface ILibraryRepository
{
    Task AddAsync(Library library);
    Task UpdateAsync(Library library);
    Task DeleteAsync(int id);
    Task<Library?> GetLibraryByIdAsync(int id);
    Task<Library?> GetLibraryByUserIdAsync(int userId);
}