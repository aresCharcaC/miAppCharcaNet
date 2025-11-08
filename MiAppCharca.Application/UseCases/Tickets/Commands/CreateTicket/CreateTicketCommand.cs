using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.CreateTicket
{
    /// <summary>
    /// Comando para crear un nuevo ticket
    /// </summary>
    public class CreateTicketCommand : IRequest<TicketDto>
    {
        public Guid UserId { get; set; } // Lo setea el controller desde el token JWT
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}