using DigitalLibrary.API.Common;
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

    public async Task<Result<AuthResponseDTO>> LoginAsync(LoginDTO dto)
    {
        var user = await _userRepository.GetUserByUserNameAsync(dto.UserName);
        if (user == null ) return Result<AuthResponseDTO>.Fail(ErrorType.InvalidCredentials, "Invalid credentials");

        var ok = VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt);
        if (!ok) return Result<AuthResponseDTO>.Fail(ErrorType.InvalidCredentials, "Invalid credentials");

        var response = new AuthResponseDTO
        {
            Token = _jwtTokenService.GenerateToken(user),
            User = new UserReadDTO {Id = user.Id, UserName = user.UserName}
        };

        return Result<AuthResponseDTO>.Success(response);
    }

    public async Task<Result<AuthResponseDTO>> RegisterAsync(RegisterDTO dto)
    {
        if (await _userRepository.UserExistsAsync(dto.UserName)) return Result<AuthResponseDTO>.Fail(ErrorType.UserAlreadyExists, "User already exists");

        GeneratePasswordHash(dto.Password, out var hash, out var salt);

        var user = new User
        {
            UserName = dto.UserName,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        await _userRepository.AddAsync(user);

        var response = new AuthResponseDTO
        {
            Token = _jwtTokenService.GenerateToken(user),
            User = new UserReadDTO {Id = user.Id, UserName = user.UserName}
        };

        return Result<AuthResponseDTO>.Success(response);
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

