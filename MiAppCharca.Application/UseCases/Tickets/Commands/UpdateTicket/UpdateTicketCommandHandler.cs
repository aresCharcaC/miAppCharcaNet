using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.UpdateTicket
{
    public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, TicketDto>
    {
        private readonly ITicketRepository _ticketRepository;

        public UpdateTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<TicketDto> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket no encontrado");

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(request.Title))
                ticket.Title = request.Title;

            if (request.Description != null)
                ticket.Description = request.Description;

            if (!string.IsNullOrEmpty(request.Status))
                ticket.Status = request.Status;

            var updatedTicket = await _ticketRepository.UpdateAsync(ticket);

            return new TicketDto
            {
                TicketId = updatedTicket.TicketId,
                UserId = updatedTicket.UserId,
                Username = updatedTicket.User?.Username ?? "Unknown",
                Title = updatedTicket.Title,
                Description = updatedTicket.Description,
                Status = updatedTicket.Status,
                CreatedAt = updatedTicket.CreatedAt ?? DateTime.UtcNow,
                ClosedAt = updatedTicket.ClosedAt,
                TotalResponses = updatedTicket.Responses?.Count ?? 0
            };
        }
    }
}