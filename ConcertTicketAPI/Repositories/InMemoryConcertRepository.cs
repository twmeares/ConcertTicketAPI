using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories;

/// <summary>
/// In-memory implementation of the concert repository for testing purposes.
/// This class simulates a data store for events and tickets without using a database.
/// </summary>
public class InMemoryConcertRepository : IConcertRepository
{
    private readonly List<Event> _events;

    public InMemoryConcertRepository()
    {
        _events = new List<Event>();
    }

    public Task AddEventAsync(Event ev)
    {
        _events.Add(ev);
        return Task.CompletedTask;
    }

    public Task<Event?> UpdateEventAsync(Event ev)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == ev.Id);
        if (existingEvent != null)
        {
            //replace existing with updated event
            _events[_events.IndexOf(existingEvent)] = ev;
            return Task.FromResult<Event?>(ev);
        }
        return Task.FromResult<Event?>(null);
    }

    public Task<Event?> GetEventByIdAsync(Guid id)
    {
        var ev = _events.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(ev);
    }

    public Task<List<Event>> GetAllEventsAsync()
    {
        return Task.FromResult(_events);
    }

    public Task<List<Ticket>> GetAvailableTicketsByEventIdAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TicketResponse>> ReserveTicketsAsync(Guid eventId, List<TicketRequest> ticketRequests)
    {
        throw new NotImplementedException();
    }

    public Task<List<TicketResponse>> CancelReservationAsync(Guid eventId, List<TicketRequest> ticketRequests)
    {
        throw new NotImplementedException();
    }

    public Task<List<TicketResponse>> PurchaseTicketsAsync(Guid eventId, List<TicketRequest> ticketRequests)
    {
        throw new NotImplementedException();
    }
}
