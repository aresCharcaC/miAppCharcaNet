using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.UseCases.Tickets.Commands.CreateTicket
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, TicketDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public CreateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<TicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            // Verificar que el usuario existe
            if (!await _userRepository.ExistsAsync(request.UserId))
                throw new KeyNotFoundException("Usuario no encontrado");

            var ticket = new Ticket
            {
                TicketId = Guid.NewGuid(),
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description,
                Status = "abierto",
                CreatedAt = DateTime.UtcNow
            };

            var createdTicket = await _ticketRepository.CreateAsync(ticket);
            var user = await _userRepository.GetByIdAsync(request.UserId);

            return new TicketDto
            {
                TicketId = createdTicket.TicketId,
                UserId = createdTicket.UserId,
                Username = user?.Username ?? "Unknown",
                Title = createdTicket.Title,
                Description = createdTicket.Description,
                Status = createdTicket.Status,
                CreatedAt = createdTicket.CreatedAt ?? DateTime.UtcNow,
                ClosedAt = createdTicket.ClosedAt,
                TotalResponses = 0
            };
        }
    }
}