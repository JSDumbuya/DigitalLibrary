using DigitalLibrary.API.Data;
using DigitalLibrary.API.DTOs;
using DigitalLibrary.API.Models;

namespace DigitalLibrary.API.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthenticationService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
    {
        var user = await _userRepository.GetUserByUserNameAsync(dto.UserName);
        if (user == null ) throw new Exception("Invalid credentials");

        if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt)) throw new Exception("Invalid credentials");

        return new AuthResponseDTO
        {
            Token = _jwtTokenService.GenerateToken(user),
            User = new UserReadDTO {Id = user.Id, UserName = user.UserName}
        };
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto)
    {
        if (await _userRepository.UserExitsAsync(dto.UserName)) throw new Exception("User Already exists");

        GeneratePasswordHash(dto.Password, out var hash, out var salt);

        var user = new User
        {
            UserName = dto.UserName,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        await _userRepository.AddAsync(user);

        return new AuthResponseDTO
        {
            Token = _jwtTokenService.GenerateToken(user),
            User = new UserReadDTO {Id = user.Id, UserName = user.UserName}
        };
    }

    private static void GeneratePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(salt);
        var computed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computed.SequenceEqual(hash);
    }

}

