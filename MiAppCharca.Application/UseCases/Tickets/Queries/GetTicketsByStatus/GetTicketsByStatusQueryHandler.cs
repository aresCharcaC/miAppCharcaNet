using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Tickets.Queries.GetTicketsByStatus
{
    public class GetTicketsByStatusQueryHandler : IRequestHandler<GetTicketsByStatusQuery, IEnumerable<TicketDto>>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicketsByStatusQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<TicketDto>> Handle(GetTicketsByStatusQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetByStatusAsync(request.Status);
            
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