using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Tickets.Queries.GetAllTickets
{
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, IEnumerable<TicketDto>>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetAllTicketsQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetAllAsync();
            
            return tickets.Select(t => new TicketDto
            {
                TicketId = t.TicketId,
                UserId = t.UserId,
                Username = t.User?.Username ?? "Unknown",
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt ?? DateTime.UtcNow,
                ClosedAt = t.ClosedAt,
                TotalResponses = t.Responses?.Count ?? 0
            });
        }
    }
}