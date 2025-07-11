using System.Data.Common;
using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories;

/// <summary>
/// Interface for managing concerts (events and tickets) abstracts away the actual storage details.
/// </summary>
public interface IConcertRepository
{
    //event methods
    Task AddEventAsync(Event ev);
    Task<Event?> UpdateEventAsync(Event ev);
    Task<Event?> GetEventByIdAsync(Guid id);
    Task<List<Event>> GetAllEventsAsync();

    //ticket methods
    Task<List<Ticket>> GetAvailableTicketsByEventIdAsync(Guid eventId);
    Task<List<TicketResponse>> ReserveTicketsAsync(Guid eventId, List<TicketRequest> ticketRequests);
    Task<List<TicketResponse>> CancelReservationAsync(Guid eventId, List<TicketRequest> ticketRequests);
    Task<List<TicketResponse>> PurchaseTicketsAsync(Guid eventId, List<TicketRequest> ticketRequests);
}
