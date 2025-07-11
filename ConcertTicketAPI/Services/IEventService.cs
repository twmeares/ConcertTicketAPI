using ConcertTicketAPI.DTOs;

namespace ConcertTicketAPI.Services;

public interface IEventService
{
    Task<IEnumerable<EventResponse>> GetAllEventsAsync();
    Task<EventResponse?> GetByIdAsync(Guid id);
}