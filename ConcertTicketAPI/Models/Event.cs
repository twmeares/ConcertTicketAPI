namespace ConcertTicketAPI.Models;

public class Event
{
    public Event()
    {
        
    }

    public Guid Id { get; set; }
    public DateTime date { get; set; }
    public required string Venue { get; set; }
    public required string Description { get; set; }
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public int TicketsRemaining { get; set; }
    public List<TicketTypes> TicketTypes { get; set; } = new List<TicketTypes>();

}