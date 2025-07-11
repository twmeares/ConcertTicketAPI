using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;

namespace ConcertTicketAPI.Services;

public class EventService : IEventService
{
    private readonly ILogger<EventService> _logger;
    private readonly IConcertRepository _eventRepository;

    public EventService(ILogger<EventService> logger, IConcertRepository eventRepository)
    {
        _logger = logger;
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventResponse>> GetAllEventsAsync()
    {
        var events = await _eventRepository.GetAllEventsAsync(); 
        if (events == null || !events.Any())
        {
            return Enumerable.Empty<EventResponse>();
        }

        // map event to eventResponse
        List<EventResponse> responses = new List<EventResponse>();
        foreach (var ev in events)
        {
            responses.Add(new EventResponse
            {
                Id = ev.Id,
                Name = ev.Name,
                Description = ev.Description,
                Venue = ev.Venue,
                date = ev.date,
                TicketTypes = ev.TicketTypes
            });
        }
        return responses;
    }

    public async Task<EventResponse?> GetByIdAsync(Guid id)
    {
        var ev = await _eventRepository.GetEventByIdAsync(id);
        if (ev == null)
        {
            return null;
        }

        
        var response = new EventResponse
        {
            Id = ev.Id,
            Name = ev.Name,
            Description = ev.Description,
            Venue = ev.Venue,
            date = ev.date,
            TicketTypes = ev.TicketTypes
        };
        return response;
    }
    
}