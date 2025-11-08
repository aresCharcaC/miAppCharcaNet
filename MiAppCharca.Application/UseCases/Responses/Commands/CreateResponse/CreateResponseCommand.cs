using MediatR;
using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.UseCases.Responses.Commands.CreateResponse
{
    /// <summary>
    /// Comando para crear una respuesta a un ticket
    /// </summary>
    public class CreateResponseCommand : IRequest<ResponseDto>
    {
        public Guid ResponderId { get; set; } // Lo setea el controller desde el token JWT
        public Guid TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}