using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Responses.Queries.GetResponseById
{
    /// <summary>
    /// Query para obtener una respuesta por ID
    /// </summary>
    public class GetResponseByIdQuery : IRequest<ResponseDto?>
    {
        public Guid ResponseId { get; set; }
    }
}