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
using System;

namespace ConcertTicketAPI.Tests;

public class TicketsControllerTests
{
    public TicketsControllerTests()
    {

    }

    [Fact]
    public async Task Purchase_ShouldReturnSuccess_WhenTicketReserved()
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
        // create test tickets reserved by the user
        var userId = Guid.NewGuid();
        var tickets = new List<Ticket>
        {
            new Ticket { Id = Guid.NewGuid(), EventId = eventId, UserId = userId,
                ReservedUntil = DateTime.UtcNow.AddMinutes(15), Price = 50, TicketType = TicketTypes.GeneralAdmission },
        };
        await concertRepository.AddTicketsAsync(tickets);

        var controller = new TicketsController(new LoggerFactory().CreateLogger<TicketsController>(), ticketService);

        var result = await controller.PurchaseTickets(new TicketRequest
        {
            EventId = eventId,  // Add the EventId that was missing
            UserId = userId,
            TicketIds = tickets.Select(t => t.Id).ToList()
        });
        Console.WriteLine(result);
        if (result is BadRequestObjectResult badRequest)
        {
            Console.WriteLine($"BadRequest returned with message: {badRequest.Value}");
        }
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<TicketTransactionResponse>(okResult.Value);
        Assert.True(response.Success);
        
    }
    
    //TODO create test for other methods in TicketsController
}
