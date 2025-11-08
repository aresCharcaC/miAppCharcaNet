using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Tickets.Queries.GetTicketsByStatus
{
    /// <summary>
    /// Query para obtener tickets por estado
    /// </summary>
    public class GetTicketsByStatusQuery : IRequest<IEnumerable<TicketDto>>
    {
        public string Status { get; set; } = string.Empty;
    }
}