using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email ?? string.Empty,
                CreatedAt = u.CreatedAt ?? DateTime.UtcNow,
                Roles = u.UserRoles?.Select(ur => ur.Role?.RoleName ?? string.Empty).ToList() ?? new List<string>()
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email ?? string.Empty,
                CreatedAt = user.CreatedAt ?? DateTime.UtcNow,
                Roles = user.UserRoles?.Select(ur => ur.Role?.RoleName ?? string.Empty).ToList() ?? new List<string>()
            };
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email ?? string.Empty,
                CreatedAt = user.CreatedAt ?? DateTime.UtcNow,
                Roles = user.UserRoles?.Select(ur => ur.Role?.RoleName ?? string.Empty).ToList() ?? new List<string>()
            };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            // Verificar si el usuario ya existe
            if (await _userRepository.UsernameExistsAsync(dto.Username))
                throw new InvalidOperationException("El nombre de usuario ya está en uso");

            // Encriptar contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = dto.Username,
                PasswordHash = passwordHash,
                Email = dto.Email,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = await _userRepository.CreateAsync(user);

            return new UserDto
            {
                UserId = createdUser.UserId,
                Username = createdUser.Username,
                Email = createdUser.Email ?? string.Empty,
                CreatedAt = createdUser.CreatedAt ?? DateTime.UtcNow,
                Roles = new List<string>()
            };
        }

        public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            // Actualizar email si se proporciona
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            // Actualizar contraseña si se proporciona
            if (!string.IsNullOrEmpty(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var updatedUser = await _userRepository.UpdateAsync(user);

            return new UserDto
            {
                UserId = updatedUser.UserId,
                Username = updatedUser.Username,
                Email = updatedUser.Email ?? string.Empty,
                CreatedAt = updatedUser.CreatedAt ?? DateTime.UtcNow,
                Roles = updatedUser.UserRoles?.Select(ur => ur.Role?.RoleName ?? string.Empty).ToList() ?? new List<string>()
            };
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }
    }
}