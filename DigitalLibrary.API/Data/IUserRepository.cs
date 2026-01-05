namespace DigitalLibrary.API.Data;

using DigitalLibrary.API.Models;
public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByUserNameAsync(string username);
    Task<bool> UserExistsAsync(string username);
}