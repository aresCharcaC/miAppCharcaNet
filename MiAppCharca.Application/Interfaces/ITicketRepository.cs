using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(Guid ticketId);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Ticket>> GetByStatusAsync(string status);
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket> UpdateAsync(Ticket ticket);
        Task<bool> DeleteAsync(Guid ticketId);
        Task<bool> ExistsAsync(Guid ticketId);
        Task<int> GetTotalTicketsByUserAsync(Guid userId);
    }
}