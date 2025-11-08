using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Responses.Commands.UpdateResponse
{
    public class UpdateResponseCommandHandler : IRequestHandler<UpdateResponseCommand, ResponseDto>
    {
        private readonly IResponseRepository _responseRepository;

        public UpdateResponseCommandHandler(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public async Task<ResponseDto> Handle(UpdateResponseCommand request, CancellationToken cancellationToken)
        {
            var response = await _responseRepository.GetByIdAsync(request.ResponseId);
            if (response == null)
                throw new KeyNotFoundException("Respuesta no encontrada");

            response.Message = request.Message;

            var updatedResponse = await _responseRepository.UpdateAsync(response);

            return new ResponseDto
            {
                ResponseId = updatedResponse.ResponseId,
                TicketId = updatedResponse.TicketId,
                ResponderId = updatedResponse.ResponderId,
                ResponderUsername = updatedResponse.Responder?.Username ?? "Unknown",
                Message = updatedResponse.Message,
                CreatedAt = updatedResponse.CreatedAt ?? DateTime.UtcNow
            };
        }
    }
}