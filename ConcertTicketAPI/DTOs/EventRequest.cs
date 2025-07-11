using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.DTOs;
public class EventRequest
{
    // Nullable Guild to allow using the EventRequest for both creation and updates
    // If Id is null, it indicates a new event creation
    public Guid? Id { get; set; }
    public DateTime date { get; set; }
    public required string Venue { get; set; }
    public required string Description { get; set; }
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public int TicketsRemaining { get; set; }
    public List<TicketTypes> TicketTypes { get; set; } = new List<TicketTypes>();
}