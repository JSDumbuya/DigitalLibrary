namespace DigitalLibrary.API.Services;

using DigitalLibrary.API.Common;
using DigitalLibrary.API.DTOs;
public interface IUserService
{
    Task<Result<UserReadDTO>> GetUserByIdAsync(int id);
}