using ConcertTicketAPI.DTOs;
using Microsoft.AspNetCore.Http;

namespace ConcertTicketAPI.Services;

public interface IEventService
{
    Task<IEnumerable<EventResponse>> GetAllEventsAsync();
    Task<EventResponse?> GetByIdAsync(Guid id);
    Task<EventResponse> CreateEvent(EventRequest request);
    Task<EventResponse?> UpdateEvent(EventRequest request);
}