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

    public Task<EventResponse> CreateEvent(EventRequest request)
    {
        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Venue = request.Venue,
            date = request.date,
            TicketTypes = request.TicketTypes
        };

        _eventRepository.AddEventAsync(newEvent);
        
        return Task.FromResult(new EventResponse
        {
            Id = newEvent.Id,
            Name = newEvent.Name,
            Description = newEvent.Description,
            Venue = newEvent.Venue,
            date = newEvent.date,
            TicketTypes = newEvent.TicketTypes
        });
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

    public async Task<EventResponse?> UpdateEvent(EventRequest request)
    {
        var existingEvent = new Event
        {
            Id = request.Id ?? Guid.Empty, // Ensure Id is not null
            Name = request.Name,
            Description = request.Description,
            Venue = request.Venue,
            date = request.date,
            TicketTypes = request.TicketTypes
        };

        var updatedEvent = await _eventRepository.UpdateEventAsync(existingEvent);
        if (updatedEvent == null)
        {
            return null;
        }

        return new EventResponse
        {
            Id = updatedEvent.Id,
            Name = updatedEvent.Name,
            Description = updatedEvent.Description,
            Venue = updatedEvent.Venue,
            date = updatedEvent.date,
            TicketTypes = updatedEvent.TicketTypes
        };
    }
}