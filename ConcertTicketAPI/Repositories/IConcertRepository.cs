using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories;

/// <summary>
/// Interface for managing concerts (events and tickets) abstracts away the actual storage details.
/// </summary>
public interface IConcertRepository
{
    void AddEvent(Event ev);
    Task<Event?> GetEventByIdAsync(Guid id);
    Task<List<Event>> GetAllEventsAsync();
}
