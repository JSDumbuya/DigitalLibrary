namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
public interface IUserService
{
    Task<User> AddUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> UpdateUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
}