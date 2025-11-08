using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto dto);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}