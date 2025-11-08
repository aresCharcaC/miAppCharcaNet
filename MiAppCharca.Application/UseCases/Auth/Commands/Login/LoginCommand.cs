using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Auth.Commands.Login
{
    /// <summary>
    /// Comando para iniciar sesión
    /// IRequest<LoginResponseDto> indica que este comando retorna un LoginResponseDto
    /// </summary>
    public class LoginCommand : IRequest<LoginResponseDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}