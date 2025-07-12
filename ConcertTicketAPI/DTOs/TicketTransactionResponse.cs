namespace ConcertTicketAPI.DTOs;

public class TicketTransactionResponse
{
    public bool Success { get; set; }
    public List<Guid>? ReservedTicketIds { get; set; }
    public List<Guid>? UnavailableTicketIds { get; set; }
}