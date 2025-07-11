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

public class EventsAPITests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public EventsAPITests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllEvents_ShouldReturnEmptyList_WhenNoEvents()
    {
        // end to end test of the API using the injected in-memory repository
        // by default the event repository is empty
        var response = await _client.GetAsync("/Events");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(json));
        var events = JsonSerializer.Deserialize<List<EventResponse>>(json);
        Assert.True(events != null);
        Assert.True(events.Count == 0);
    }

    
}
