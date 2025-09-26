namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
public interface IUserService
{
    Task AddUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task UpdateUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
}