using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.DTOs;
public class CreateTicketRequest
{
    public Guid EventId { get; set; }
    
    public TicketTypes TicketType { get; set; }
    public decimal Price { get; set; }

    // row and seat are optional, depending on the ticketType
    public string Row { get; set; } = string.Empty;
    public string Seat { get; set; } = string.Empty;
}