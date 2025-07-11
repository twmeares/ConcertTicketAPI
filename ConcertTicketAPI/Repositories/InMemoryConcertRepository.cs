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

    public void AddEvent(Event ev)
    {
        _events.Add(ev);
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
}
