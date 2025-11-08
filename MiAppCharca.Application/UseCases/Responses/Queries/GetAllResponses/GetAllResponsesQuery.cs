using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Responses.Queries.GetAllResponses
{
    /// <summary>
    /// Query para obtener todas las respuestas (Admin/Support)
    /// </summary>
    public class GetAllResponsesQuery : IRequest<IEnumerable<ResponseDto>>
    {
    }
}