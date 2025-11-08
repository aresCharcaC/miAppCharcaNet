using MediatR;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.CloseTicket
{
    public class CloseTicketCommandHandler : IRequestHandler<CloseTicketCommand, bool>
    {
        private readonly ITicketRepository _ticketRepository;

        public CloseTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> Handle(CloseTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket no encontrado");

            ticket.Status = "cerrado";
            ticket.ClosedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            return true;
        }
    }
}