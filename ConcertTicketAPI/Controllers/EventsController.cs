using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;

    public EventsController(ILogger<EventsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> GetAll()
    {
        //TODO: stub for basic compile test for now. Come back and fix this.
        return ["event1", "event2"];
    }
}
