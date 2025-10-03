namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.Data;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> AddUserAsync(User user)
    {
        return _userRepository.AddAsync(user);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        return _userRepository.DeleteAsync(id);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        return _userRepository.GetUserByIdAsync(id);
    }

    public Task<bool> UpdateUserAsync(User user)
    {
        return _userRepository.UpdateAsync(user);
    }
}