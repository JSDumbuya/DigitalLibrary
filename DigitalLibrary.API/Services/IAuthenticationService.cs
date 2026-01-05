using DigitalLibrary.API.DTOs;

namespace DigitalLibrary.API.Services;

public interface IAuthenticationService
{
    Task<AuthResponseDTO> RegisterAsync(RegisterDTO dto);
    Task<AuthResponseDTO> LoginAsync(LoginDTO dto);
}