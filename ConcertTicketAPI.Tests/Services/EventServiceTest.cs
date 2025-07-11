using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Microsoft.Extensions.Logging;

namespace ConcertTicketAPI.Tests;

public class EventServiceTest
{
    [Fact]
    public async Task GetByIdAsync_ReturnsEvent_WhenEventExists()
    {
        // init deps
        var eventRepository = new InMemoryConcertRepository();
        var eventService = new EventService(new LoggerFactory().CreateLogger<EventService>(), eventRepository);

        // create test event
        var eventId = Guid.NewGuid();
        var testEvent = new Event
        {
            Id = eventId,
            Name = "Test Concert",
            Venue = "Test Venue",
            Description = "Test Description",
            date = DateTime.Now,
            Capacity = 100,
            TicketsRemaining = 50
        };

        eventRepository.AddEvent(testEvent);

        var result = await eventService.GetByIdAsync(eventId);

        Assert.NotNull(result);
        Assert.Equal(testEvent.Id, result?.Id);
    }
    
    //TODO create tests for GetAllEventsAsync and other methods
}