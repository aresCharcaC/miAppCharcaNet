using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Responses.Queries.GetAllResponses
{
    public class GetAllResponsesQueryHandler : IRequestHandler<GetAllResponsesQuery, IEnumerable<ResponseDto>>
    {
        private readonly IResponseRepository _responseRepository;

        public GetAllResponsesQueryHandler(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public async Task<IEnumerable<ResponseDto>> Handle(GetAllResponsesQuery request, CancellationToken cancellationToken)
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
    }
}