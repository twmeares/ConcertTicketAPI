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
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace ConcertTicketAPI.Tests;

public class ConcertTicketAPITests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    public ConcertTicketAPITests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetAllEvents_ShouldReturnEmptyList_WhenNoEvents()
    {
        // end to end test of the API using the injected in-memory repository
        // clear the repository to ensure no events exist
        var repo = _factory.Services.GetRequiredService<IConcertRepository>() as InMemoryConcertRepository;
        repo?.Clear();

        var response = await _client.GetAsync("/Events");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(json));
        var events = JsonSerializer.Deserialize<List<EventResponse>>(json);
        Assert.True(events != null);
        Assert.True(events.Count == 0);
    }

    [Fact]
    public async Task TicketLifecycleFlowReserveAndCancel_ShouldSucceed()
    {
        // create event
        var eventId = Guid.NewGuid();
        var testEvent = new EventRequest
        {
            Id = eventId,
            Name = "Test Concert",
            Venue = "Test Venue",
            Description = "Test Description",
            date = DateTime.UtcNow,
            Capacity = 100,
            TicketsRemaining = 50,
            TicketTypes = new List<TicketTypes> { TicketTypes.GeneralAdmission }
        };
        var createEventResponse = await _client.PostAsJsonAsync("/Events", testEvent);
        createEventResponse.EnsureSuccessStatusCode();

        var createdEvent = await createEventResponse.Content.ReadFromJsonAsync<EventResponse>();
        Assert.NotNull(createdEvent);

        // add tickets to event
        var ticketId = Guid.NewGuid();
        List<CreateTicketRequest> tickets = new List<CreateTicketRequest>()
        {
            new CreateTicketRequest(){
                EventId = eventId,
                Price = 50,
                TicketType = TicketTypes.GeneralAdmission,
            }
        };
        var createTicketResponse = await _client.PostAsJsonAsync("/Tickets/create-tickets", tickets);
        createTicketResponse.EnsureSuccessStatusCode();

        var createdTickets = await createTicketResponse.Content.ReadFromJsonAsync<List<TicketResponse>>();
        Assert.NotNull(createdTickets);

        // query available tickets
        var availableTicketsResponse = await _client.GetAsync($"/Tickets/{eventId}");
        availableTicketsResponse.EnsureSuccessStatusCode();

        var availableTickets = await availableTicketsResponse.Content.ReadFromJsonAsync<List<TicketResponse>>();
        Assert.NotNull(availableTickets);
        Assert.True(availableTickets.Count > 0);

        // reserve a ticket
        var userId = Guid.NewGuid();
        var reserveRequest = new TicketRequest
        {
            EventId = eventId,
            UserId = userId,
            TicketIds = availableTickets.Select(t => t.Id).ToList()
        };
        var reserveResponse = await _client.PostAsJsonAsync("/Tickets/reserve", reserveRequest);
        reserveResponse.EnsureSuccessStatusCode();

        var reserveResult = await reserveResponse.Content.ReadFromJsonAsync<TicketTransactionResponse>();
        Assert.NotNull(reserveResult);
        Assert.True(reserveResult.Success);

        // cancel ticket reservation
        var cancelResponse = await _client.PostAsJsonAsync("/Tickets/cancel-reservation", reserveRequest);
        cancelResponse.EnsureSuccessStatusCode();

        var cancelResult = await cancelResponse.Content.ReadFromJsonAsync<TicketTransactionResponse>();
        Assert.NotNull(cancelResult);
        Assert.True(cancelResult.Success);
    }
}
