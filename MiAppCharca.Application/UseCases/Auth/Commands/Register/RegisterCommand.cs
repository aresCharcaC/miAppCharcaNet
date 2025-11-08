using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Auth.Commands.Register
{
    /// <summary>
    /// Comando para registrar un nuevo usuario
    /// </summary>
    public class RegisterCommand : IRequest<RegisterResponseDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = "User"; // Rol por defecto
    }
}