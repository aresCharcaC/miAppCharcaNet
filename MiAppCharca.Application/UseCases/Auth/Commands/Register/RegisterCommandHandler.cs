using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.UseCases.Auth.Commands.Register
{
    /// <summary>
    /// Handler que procesa el comando de Register
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Verificar si el usuario ya existe
            if (await _userRepository.UsernameExistsAsync(request.Username))
                throw new InvalidOperationException("El nombre de usuario ya está en uso");

            // Encriptar contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Crear usuario
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);

            // Asignar rol por defecto
            var role = await _roleRepository.GetByNameAsync(request.RoleName);
            if (role != null)
            {
                // Usar el método del repositorio para asignar el rol
                await _userRepository.AssignRoleToUserAsync(user.UserId, role.RoleId);
            }

            return new RegisterResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email ?? string.Empty,
                Message = $"Usuario registrado exitosamente con rol: {request.RoleName}"
            };
        }
    }
}