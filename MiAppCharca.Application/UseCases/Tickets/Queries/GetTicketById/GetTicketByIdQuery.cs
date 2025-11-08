using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Tickets.Queries.GetTicketById
{
    /// <summary>
    /// Query para obtener un ticket por ID con sus respuestas
    /// </summary>
    public class GetTicketByIdQuery : IRequest<TicketDetailDto?>
    {
        public Guid TicketId { get; set; }
    }
}