using Microsoft.EntityFrameworkCore;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;
using MiAppCharca.Infrastructure.Data;

namespace MiAppCharca.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketeraDbContext _context;

        public TicketRepository(TicketeraDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket?> GetByIdAsync(Guid ticketId)
        {
            return await _context.Tickets
                .Include(t => t.User)
                .Include(t => t.Responses)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .Include(t => t.Responses)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetByStatusAsync(string status)
        {
            return await _context.Tickets
                .Where(t => t.Status == status)
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket> UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<bool> DeleteAsync(Guid ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid ticketId)
        {
            return await _context.Tickets.AnyAsync(t => t.TicketId == ticketId);
        }

        public async Task<int> GetTotalTicketsByUserAsync(Guid userId)
        {
            return await _context.Tickets.CountAsync(t => t.UserId == userId);
        }
    }
}