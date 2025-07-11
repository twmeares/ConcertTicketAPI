namespace ConcertTicketAPI.DTOs;

//exposing only the minimum information here
// this DTO will be used for reserve, purchase, cancel
public class TicketRequest
{
    public Guid EventId { get; set; }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

}