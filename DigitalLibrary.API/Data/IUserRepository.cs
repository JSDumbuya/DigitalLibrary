namespace DigitalLibrary.Data;

using DigitalLibrary.Models;
public interface IUserRepository
{
    Task AddAsync(User user);
    Task DeleteAsync(int id);
    Task UpdateAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
}