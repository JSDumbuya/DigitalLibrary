namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserReadDTO?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return user == null ? null : MapperUserToReadDTO(user);
    }

    private UserReadDTO MapperUserToReadDTO(User user)
    {
        return new UserReadDTO
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }

}