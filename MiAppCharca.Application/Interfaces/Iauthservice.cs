using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<bool> ValidateUserAsync(string username, string password);
        string GenerateJwtToken(Guid userId, string username, List<string> roles);
    }
}