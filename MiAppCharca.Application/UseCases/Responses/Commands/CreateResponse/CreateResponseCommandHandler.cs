using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.UseCases.Responses.Commands.CreateResponse
{
    public class CreateResponseCommandHandler : IRequestHandler<CreateResponseCommand, ResponseDto>
    {
        private readonly IResponseRepository _responseRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public CreateResponseCommandHandler(
            IResponseRepository responseRepository,
            ITicketRepository ticketRepository,
            IUserRepository userRepository)
        {
            _responseRepository = responseRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<ResponseDto> Handle(CreateResponseCommand request, CancellationToken cancellationToken)
        {
            // Verificar que el ticket existe
            if (!await _ticketRepository.ExistsAsync(request.TicketId))
                throw new KeyNotFoundException("Ticket no encontrado");

            // Verificar que el usuario existe
            if (!await _userRepository.ExistsAsync(request.ResponderId))
                throw new KeyNotFoundException("Usuario no encontrado");

            var response = new Response
            {
                ResponseId = Guid.NewGuid(),
                TicketId = request.TicketId,
                ResponderId = request.ResponderId,
                Message = request.Message,
                CreatedAt = DateTime.UtcNow
            };

            var createdResponse = await _responseRepository.CreateAsync(response);
            var responder = await _userRepository.GetByIdAsync(request.ResponderId);

            return new ResponseDto
            {
                ResponseId = createdResponse.ResponseId,
                TicketId = createdResponse.TicketId,
                ResponderId = createdResponse.ResponderId,
                ResponderUsername = responder?.Username ?? "Unknown",
                Message = createdResponse.Message,
                CreatedAt = createdResponse.CreatedAt ?? DateTime.UtcNow
            };
        }
    }
}