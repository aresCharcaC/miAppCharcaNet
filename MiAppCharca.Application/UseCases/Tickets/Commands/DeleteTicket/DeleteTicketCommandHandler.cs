using MediatR;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.DeleteTicket
{
    public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
    {
        private readonly ITicketRepository _ticketRepository;

        public DeleteTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            return await _ticketRepository.DeleteAsync(request.TicketId);
        }
    }
}