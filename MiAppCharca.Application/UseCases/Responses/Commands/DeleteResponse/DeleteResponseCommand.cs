using MediatR;

namespace MiAppCharca.Application.UseCases.Responses.Commands.DeleteResponse
{
    /// <summary>
    /// Comando para eliminar una respuesta
    /// </summary>
    public class DeleteResponseCommand : IRequest<bool>
    {
        public Guid ResponseId { get; set; }
    }
}