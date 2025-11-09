namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<bool> DeleteAsync(int id);
    Task<bool> UpdateAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUserNameAsync(string username);
    Task<bool> UserExitsAsync(string username);
}