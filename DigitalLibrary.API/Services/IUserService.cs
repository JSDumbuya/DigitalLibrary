namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.DTOs;
public interface IUserService
{
    Task<UserReadDTO?> GetUserByIdAsync(int id);
}