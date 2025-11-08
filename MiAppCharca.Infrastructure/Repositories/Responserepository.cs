using Microsoft.EntityFrameworkCore;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;
using MiAppCharca.Infrastructure.Data;

namespace MiAppCharca.Infrastructure.Repositories
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly TicketeraDbContext _context;

        public ResponseRepository(TicketeraDbContext context)
        {
            _context = context;
        }

        public async Task<Response?> GetByIdAsync(Guid responseId)
        {
            return await _context.Responses
                .Include(r => r.Ticket)
                .Include(r => r.Responder)
                .FirstOrDefaultAsync(r => r.ResponseId == responseId);
        }

        public async Task<IEnumerable<Response>> GetAllAsync()
        {
            return await _context.Responses
                .Include(r => r.Ticket)
                .Include(r => r.Responder)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Response>> GetByTicketIdAsync(Guid ticketId)
        {
            return await _context.Responses
                .Where(r => r.TicketId == ticketId)
                .Include(r => r.Responder)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Response>> GetByResponderIdAsync(Guid responderId)
        {
            return await _context.Responses
                .Where(r => r.ResponderId == responderId)
                .Include(r => r.Ticket)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Response> CreateAsync(Response response)
        {
            await _context.Responses.AddAsync(response);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<Response> UpdateAsync(Response response)
        {
            _context.Responses.Update(response);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<bool> DeleteAsync(Guid responseId)
        {
            var response = await _context.Responses.FindAsync(responseId);
            if (response == null) return false;

            _context.Responses.Remove(response);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalResponsesByTicketAsync(Guid ticketId)
        {
            return await _context.Responses.CountAsync(r => r.TicketId == ticketId);
        }
    }
}