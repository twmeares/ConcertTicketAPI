using System.Text.Json.Serialization;
using ConcertTicketAPI.Models;

// abstracts event to hide potential sensitive data
// and to make potential db changes not affect the API
namespace ConcertTicketAPI.DTOs;
public class EventResponse
{
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("venue")]
    public required string Venue { get; set; }

    [JsonPropertyName("date")]
    public DateTime date { get; set; }

    [JsonPropertyName("ticketTypes")]
    public List<TicketTypes> TicketTypes { get; set; } = new List<TicketTypes>();
}