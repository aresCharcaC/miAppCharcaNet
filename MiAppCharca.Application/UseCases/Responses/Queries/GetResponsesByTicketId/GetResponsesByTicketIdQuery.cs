using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Responses.Queries.GetResponsesByTicketId
{
    /// <summary>
    /// Query para obtener todas las respuestas de un ticket
    /// </summary>
    public class GetResponsesByTicketIdQuery : IRequest<IEnumerable<ResponseDto>>
    {
        public Guid TicketId { get; set; }
    }
}