using Microsoft.EntityFrameworkCore;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;
using MiAppCharca.Infrastructure.Data;

namespace MiAppCharca.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly TicketeraDbContext _context;

        public RoleRepository(TicketeraDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByIdAsync(Guid roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> CreateAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteAsync(Guid roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetRolesByUserIdAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
        }
    }
}