using System.Data.Common;
using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories;

/// <summary>
/// Interface for managing concerts (events and tickets) abstracts away the actual storage details.
/// </summary>
public interface IConcertRepository
{
    Task AddEventAsync(Event ev);
    Task<Event?> UpdateEventAsync(Event ev);
    Task<Event?> GetEventByIdAsync(Guid id);
    Task<List<Event>> GetAllEventsAsync();
}
