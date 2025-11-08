using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
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

        public async Task<TicketDetailDto?> GetTicketByIdAsync(Guid ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
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

        public async Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(Guid userId)
        {
            var tickets = await _ticketRepository.GetByUserIdAsync(userId);
            
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

        public async Task<IEnumerable<TicketDto>> GetTicketsByStatusAsync(string status)
        {
            var tickets = await _ticketRepository.GetByStatusAsync(status);
            
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

        public async Task<TicketDto> CreateTicketAsync(Guid userId, CreateTicketDto dto)
        {
            // Verificar que el usuario existe
            if (!await _userRepository.ExistsAsync(userId))
                throw new KeyNotFoundException("Usuario no encontrado");

            var ticket = new Ticket
            {
                TicketId = Guid.NewGuid(),
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                Status = "abierto",
                CreatedAt = DateTime.UtcNow
            };

            var createdTicket = await _ticketRepository.CreateAsync(ticket);
            var user = await _userRepository.GetByIdAsync(userId);

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

        public async Task<TicketDto> UpdateTicketAsync(Guid ticketId, UpdateTicketDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket no encontrado");

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(dto.Title))
                ticket.Title = dto.Title;

            if (dto.Description != null)
                ticket.Description = dto.Description;

            if (!string.IsNullOrEmpty(dto.Status))
                ticket.Status = dto.Status;

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

        public async Task<bool> DeleteTicketAsync(Guid ticketId)
        {
            return await _ticketRepository.DeleteAsync(ticketId);
        }

        public async Task<bool> CloseTicketAsync(Guid ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                throw new KeyNotFoundException("Ticket no encontrado");

            ticket.Status = "cerrado";
            ticket.ClosedAt = DateTime.UtcNow;

            await _ticketRepository.UpdateAsync(ticket);
            return true;
        }
    }
}