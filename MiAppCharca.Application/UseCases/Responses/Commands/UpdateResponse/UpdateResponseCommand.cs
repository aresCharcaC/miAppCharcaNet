using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Responses.Commands.UpdateResponse
{
    /// <summary>
    /// Comando para actualizar una respuesta
    /// </summary>
    public class UpdateResponseCommand : IRequest<ResponseDto>
    {
        public Guid ResponseId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}