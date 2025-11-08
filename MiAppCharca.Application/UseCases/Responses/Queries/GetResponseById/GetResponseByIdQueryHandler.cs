using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Responses.Queries.GetResponseById
{
    public class GetResponseByIdQueryHandler : IRequestHandler<GetResponseByIdQuery, ResponseDto?>
    {
        private readonly IResponseRepository _responseRepository;

        public GetResponseByIdQueryHandler(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public async Task<ResponseDto?> Handle(GetResponseByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _responseRepository.GetByIdAsync(request.ResponseId);
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
    }
}