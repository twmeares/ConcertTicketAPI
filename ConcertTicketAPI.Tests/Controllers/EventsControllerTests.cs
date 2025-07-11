using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ConcertTicketAPI;
using System.Text.Json;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;
using Microsoft.Extensions.Logging;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using ConcertTicketAPI.DTOs;

namespace ConcertTicketAPI.Tests;

public class EventsControllerTests
{
    public EventsControllerTests()
    {

    }

    [Fact]
    public async Task GetById_ShouldReturnEvent_WhenEventsExist()
    {
        Guid eventId = Guid.NewGuid();
        var ev = new Event()
        {
            Id = eventId,
            Name = "Test Event",
            Description = "Test Description",
            Venue = "Test Venue",
            date = DateTime.Now,
            Capacity = 100,
            TicketsRemaining = 50,
            TicketTypes = new List<TicketTypes>() { TicketTypes.GeneralAdmission }
        };

        var repo = new InMemoryConcertRepository();
        await repo.AddEventAsync(ev);

        var eventService = new EventService(new LoggerFactory().CreateLogger<EventService>(), repo);

        var controller = new EventsController(new LoggerFactory().CreateLogger<EventsController>(), eventService);

        var result = await controller.GetById(eventId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<EventResponse>(okResult.Value);
        Assert.Equal("Test Event", response.Name);
        Assert.Contains(response.TicketTypes, t => t == TicketTypes.GeneralAdmission);
    }
    
    //TODO create test for other methods in EventsController
}
