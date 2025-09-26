namespace DigitalLibrary.Services;

using DigitalLibrary.Models;
using DigitalLibrary.Data;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task AddUserAsync(User user)
    {
        return _userRepository.AddAsync(user);
    }

    public Task DeleteUserAsync(int id)
    {
        return _userRepository.DeleteAsync(id);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        return _userRepository.GetUserByIdAsync(id);
    }

    public Task UpdateUserAsync(User user)
    {
        return _userRepository.UpdateAsync(user);
    }
}