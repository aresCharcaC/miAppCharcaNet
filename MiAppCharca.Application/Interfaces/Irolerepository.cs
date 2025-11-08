using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(Guid roleId);
        Task<Role?> GetByNameAsync(string roleName);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> CreateAsync(Role role);
        Task<bool> DeleteAsync(Guid roleId);
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId);
    }
}