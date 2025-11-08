using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;
using MiAppCharca.Infrastructure;

namespace MiAppCharca.Application.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public ResponseService(
            IResponseRepository responseRepository,
            ITicketRepository ticketRepository,
            IUserRepository userRepository)
        {
            _responseRepository = responseRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ResponseDto>> GetAllResponsesAsync()
        {
            var responses = await _responseRepository.GetAllAsync();
            
            return responses.Select(r => new ResponseDto
            {
                ResponseId = r.ResponseId,
                TicketId = r.TicketId,
                ResponderId = r.ResponderId,
                ResponderUsername = r.Responder?.Username ?? "Unknown",
                Message = r.Message,
                CreatedAt = r.CreatedAt ?? DateTime.UtcNow
            });
        }

        public async Task<ResponseDto?> GetResponseByIdAsync(Guid responseId)
        {
            var response = await _responseRepository.GetByIdAsync(responseId);
            if (response == null) return null;

            return new ResponseDto
            {
                ResponseId = response.ResponseId,
                TicketId = response.TicketId,
                ResponderId = response.ResponderId,
                ResponderUsername = response.Responder?.Username ?? "Unknown",
                Message = response.Message,
                CreatedAt = response.CreatedAt ?? DateTime.UtcNow
            };
        }

        public async Task<IEnumerable<ResponseDto>> GetResponsesByTicketIdAsync(Guid ticketId)
        {
            var responses = await _responseRepository.GetByTicketIdAsync(ticketId);
            
            return responses.Select(r => new ResponseDto
            {
                ResponseId = r.ResponseId,
                TicketId = r.TicketId,
                ResponderId = r.ResponderId,
                ResponderUsername = r.Responder?.Username ?? "Unknown",
                Message = r.Message,
                CreatedAt = r.CreatedAt ?? DateTime.UtcNow
            });
        }

        public async Task<ResponseDto> CreateResponseAsync(Guid responderId, CreateResponseDto dto)
        {
            // Verificar que el ticket existe
            if (!await _ticketRepository.ExistsAsync(dto.TicketId))
                throw new KeyNotFoundException("Ticket no encontrado");

            // Verificar que el usuario existe
            if (!await _userRepository.ExistsAsync(responderId))
                throw new KeyNotFoundException("Usuario no encontrado");

            var response = new Response
            {
                ResponseId = Guid.NewGuid(),
                TicketId = dto.TicketId,
                ResponderId = responderId,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow
            };

            var createdResponse = await _responseRepository.CreateAsync(response);
            var responder = await _userRepository.GetByIdAsync(responderId);

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

        public async Task<ResponseDto> UpdateResponseAsync(Guid responseId, UpdateResponseDto dto)
        {
            var response = await _responseRepository.GetByIdAsync(responseId);
            if (response == null)
                throw new KeyNotFoundException("Respuesta no encontrada");

            response.Message = dto.Message;

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

        public async Task<bool> DeleteResponseAsync(Guid responseId)
        {
            return await _responseRepository.DeleteAsync(responseId);
        }
    }
}