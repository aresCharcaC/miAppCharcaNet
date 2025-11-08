using MediatR;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.CloseTicket
{
    /// <summary>
    /// Comando para cerrar un ticket
    /// </summary>
    public class CloseTicketCommand : IRequest<bool>
    {
        public Guid TicketId { get; set; }
    }
}