using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.UpdateTicket
{
    /// <summary>
    /// Comando para actualizar un ticket
    /// </summary>
    public class UpdateTicketCommand : IRequest<TicketDto>
    {
        public Guid TicketId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}