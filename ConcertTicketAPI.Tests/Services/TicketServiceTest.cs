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
            date = DateTime.UtcNow,
            Capacity = 100,
            TicketsRemaining = 50
        };

        await concertRepository.AddEventAsync(testEvent);
        // create test tickets
        var tickets = new List<Ticket>
        {
            new Ticket { Id = Guid.NewGuid(), EventId = eventId, Price = 50, TicketType = TicketTypes.GeneralAdmission },
            new Ticket { Id = Guid.NewGuid(), EventId = eventId, Price = 50, TicketType = TicketTypes.GeneralAdmission }
        };
        await concertRepository.AddTicketsAsync(tickets);
        var result = await ticketService.GetAvailableTicketsByEventIdAsync(eventId);

        Assert.NotNull(result);
        Assert.IsType<List<TicketResponse>>(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(eventId, result[0].EventId);
    }
    
    // TODO: Add more tests the other TicketService methods
}