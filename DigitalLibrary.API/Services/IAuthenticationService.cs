using DigitalLibrary.API.Common;
using DigitalLibrary.API.DTOs;

namespace DigitalLibrary.API.Services;

public interface IAuthenticationService
{
    Task<Result<AuthResponseDTO>> RegisterAsync(RegisterDTO dto);
    Task<Result<AuthResponseDTO>> LoginAsync(LoginDTO dto);
}