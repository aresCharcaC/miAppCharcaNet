using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid userId);
        Task<bool> ExistsAsync(Guid userId);
        Task<bool> UsernameExistsAsync(string username);
        
        // NUEVO: Método para asignar rol a usuario
        Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
    }
}