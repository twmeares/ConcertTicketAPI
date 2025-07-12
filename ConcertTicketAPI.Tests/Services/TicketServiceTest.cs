using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Microsoft.Extensions.Logging;

namespace ConcertTicketAPI.Tests.Services;
public class TicketServiceTest
{
[Fact]
    public async Task GetByIdAsync_ReturnsListOfTickets_WhenEventAndTicketsExists()
    {
        // init deps
        var concertRepository = new InMemoryConcertRepository();
        var eventService = new EventService(new LoggerFactory().CreateLogger<EventService>(), concertRepository);
        var ticketService = new TicketService(new LoggerFactory().CreateLogger<TicketService>(), concertRepository);

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

        await concertRepository.AddEventAsync(testEvent);

        var result = await ticketService.GetAvailableTicketsByEventIdAsync(eventId);

        Assert.NotNull(result);
        Assert.IsType<List<TicketResponse>>(result);
    }
}