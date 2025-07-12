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
    Task<List<Ticket>> GetTicketsByTicketIdsAsync(List<Guid> ticketIds);
    Task<bool> ReserveTicketsAsync(Guid userId, List<Guid> ticketIds);
    Task<bool> CancelReservationAsync(Guid userId, List<Guid> ticketIds);
    Task<bool> CancelPurchaseAsync(Guid userId, List<Guid> ticketIds);
    Task<bool> PurchaseTicketsAsync(Guid userId, List<Guid> ticketIds);
    
}
