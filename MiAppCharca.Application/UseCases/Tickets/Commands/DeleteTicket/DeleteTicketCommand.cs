using MediatR;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.DeleteTicket
{
    /// <summary>
    /// Comando para eliminar un ticket
    /// </summary>
    public class DeleteTicketCommand : IRequest<bool>
    {
        public Guid TicketId { get; set; }
    }
}