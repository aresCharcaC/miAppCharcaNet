using MediatR;
using MiAppCharca.Application.Interfaces;

namespace MiAppCharca.Application.UseCases.Responses.Commands.DeleteResponse
{
    public class DeleteResponseCommandHandler : IRequestHandler<DeleteResponseCommand, bool>
    {
        private readonly IResponseRepository _responseRepository;

        public DeleteResponseCommandHandler(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
        }

        public async Task<bool> Handle(DeleteResponseCommand request, CancellationToken cancellationToken)
        {
            return await _responseRepository.DeleteAsync(request.ResponseId);
        }
    }
}