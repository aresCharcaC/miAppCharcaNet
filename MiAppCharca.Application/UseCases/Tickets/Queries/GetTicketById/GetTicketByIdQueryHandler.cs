using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Tickets.Queries.GetTicketById
{
    public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDetailDto?>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicketByIdQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<TicketDetailDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);
            if (ticket == null) return null;

            return new TicketDetailDto
            {
                TicketId = ticket.TicketId,
                UserId = ticket.UserId,
                Username = ticket.User?.Username ?? "Unknown",
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status,
                CreatedAt = ticket.CreatedAt ?? DateTime.UtcNow,
                ClosedAt = ticket.ClosedAt,
                Responses = ticket.Responses?.Select(r => new ResponseDto
                {
                    ResponseId = r.ResponseId,
                    TicketId = r.TicketId,
                    ResponderId = r.ResponderId,
                    ResponderUsername = r.Responder?.Username ?? "Unknown",
                    Message = r.Message,
                    CreatedAt = r.CreatedAt ?? DateTime.UtcNow
                }).ToList() ?? new List<ResponseDto>()
            };
        }
    }
}