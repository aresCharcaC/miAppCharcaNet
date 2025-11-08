using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Tickets.Queries.GetAllTickets
{
    /// <summary>
    /// Query para obtener todos los tickets
    /// </summary>
    public class GetAllTicketsQuery : IRequest<IEnumerable<TicketDto>>
    {
    }
}