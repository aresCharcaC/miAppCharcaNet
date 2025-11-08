using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Tickets.Queries.GetMyTickets
{
    /// <summary>
    /// Query para obtener los tickets del usuario actual
    /// </summary>
    public class GetMyTicketsQuery : IRequest<IEnumerable<TicketDto>>
    {
        public Guid UserId { get; set; }
    }
}