using DigitalLibrary.API.Models;

namespace DigitalLibrary.API.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}