using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ConcertTicketAPI;
using System.Text.Json;

namespace ConcertTicketAPI.Tests;

public class EventsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public EventsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllEvents_ShouldReturnListOfEvent_WhenEventsExist()
    {
        //TODO: temp stand in test for now. Come back and add the real logic.
        var response = await _client.GetAsync("/Events");

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(json));
        var events = JsonSerializer.Deserialize<List<string>>(json);
        Assert.True(events != null);
        Assert.True(events.Count > 0);
    }
}
