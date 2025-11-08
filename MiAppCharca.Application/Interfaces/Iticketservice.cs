using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<TicketDetailDto?> GetTicketByIdAsync(Guid ticketId);
        Task<IEnumerable<TicketDto>> GetTicketsByUserIdAsync(Guid userId);
        Task<IEnumerable<TicketDto>> GetTicketsByStatusAsync(string status);
        Task<TicketDto> CreateTicketAsync(Guid userId, CreateTicketDto dto);
        Task<TicketDto> UpdateTicketAsync(Guid ticketId, UpdateTicketDto dto);
        Task<bool> DeleteTicketAsync(Guid ticketId);
        Task<bool> CloseTicketAsync(Guid ticketId);
    }
}