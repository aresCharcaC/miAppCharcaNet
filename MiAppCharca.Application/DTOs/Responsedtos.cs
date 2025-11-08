namespace MiAppCharca.Application.DTOs
{
    public class ResponseDto
    {
        public Guid ResponseId { get; set; }
        public Guid TicketId { get; set; }
        public Guid ResponderId { get; set; }
        public string ResponderUsername { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateResponseDto
    {
        public Guid TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class UpdateResponseDto
    {
        public string Message { get; set; } = string.Empty;
    }
}