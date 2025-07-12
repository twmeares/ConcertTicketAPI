namespace ConcertTicketAPI.Models;

public class Ticket
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; } // DB foreign key to Event
    public Guid UserId { get; set; } // Would map as DB foreign key to User table if I had more time
    public DateTime PurchaseDate { get; set; } = DateTime.MaxValue; // Default to MaxValue to indicate not purchased yet
    public DateTime ReservedUntil { get; set; } = DateTime.MinValue ; // Default to MinValue to indicate not reserved

    // Ticket is available if reservation has expired and they have a future purchase date (aka not yet purchased)
    public bool IsAvailable => ReservedUntil < DateTime.UtcNow && PurchaseDate > DateTime.UtcNow; 
    public TicketTypes TicketType { get; set; }
    public decimal Price { get; set; }

    // row and seat are optional, depending on the ticketType
    public string Row { get; set; } = string.Empty;
    public string Seat { get; set; } = string.Empty;
}