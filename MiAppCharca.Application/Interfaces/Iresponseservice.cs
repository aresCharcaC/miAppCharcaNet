using MiAppCharca.Application.DTOs;

namespace MiAppCharca.Application.Interfaces
{
    public interface IResponseService
    {
        Task<IEnumerable<ResponseDto>> GetAllResponsesAsync();
        Task<ResponseDto?> GetResponseByIdAsync(Guid responseId);
        Task<IEnumerable<ResponseDto>> GetResponsesByTicketIdAsync(Guid ticketId);
        Task<ResponseDto> CreateResponseAsync(Guid responderId, CreateResponseDto dto);
        Task<ResponseDto> UpdateResponseAsync(Guid responseId, UpdateResponseDto dto);
        Task<bool> DeleteResponseAsync(Guid responseId);
    }
}