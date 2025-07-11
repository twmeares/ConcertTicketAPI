using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.DTOs;

public class TicketResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; } 
    public Guid UserId { get; set; } 
    public TicketTypes TicketType { get; set; }
    public decimal Price { get; set; }
    public string Row { get; set; } = string.Empty;
    public string Seat { get; set; } = string.Empty;
}