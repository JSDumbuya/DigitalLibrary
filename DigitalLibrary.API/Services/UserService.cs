namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Models;
using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Common;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserReadDTO>> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return Result<UserReadDTO>.Fail(ErrorType.UserNotFound, "The user does not exist.");

        return Result<UserReadDTO>.Success(MapperUserToReadDTO(user));
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