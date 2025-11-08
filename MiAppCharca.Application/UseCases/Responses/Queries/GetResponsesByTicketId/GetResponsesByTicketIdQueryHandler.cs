using MediatR;
using MiAppCharca.Application.DTOs;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Responses.Queries.GetResponsesByTicketId
{
    public class GetResponsesByTicketIdQueryHandler : IRequestHandler<GetResponsesByTicketIdQuery, IEnumerable<ResponseDto>>
    {
        private readonly IResponseRepository _responseRepository;

        public GetResponsesByTicketIdQueryHandler(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public async Task<IEnumerable<ResponseDto>> Handle(GetResponsesByTicketIdQuery request, CancellationToken cancellationToken)
        {
            var responses = await _responseRepository.GetByTicketIdAsync(request.TicketId);
            
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