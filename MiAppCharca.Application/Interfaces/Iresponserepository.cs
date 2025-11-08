using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Interfaces
{
    public interface IResponseRepository
    {
        Task<Response?> GetByIdAsync(Guid responseId);
        Task<IEnumerable<Response>> GetAllAsync();
        Task<IEnumerable<Response>> GetByTicketIdAsync(Guid ticketId);
        Task<IEnumerable<Response>> GetByResponderIdAsync(Guid responderId);
        Task<Response> CreateAsync(Response response);
        Task<Response> UpdateAsync(Response response);
        Task<bool> DeleteAsync(Guid responseId);
        Task<int> GetTotalResponsesByTicketAsync(Guid ticketId);
    }
}