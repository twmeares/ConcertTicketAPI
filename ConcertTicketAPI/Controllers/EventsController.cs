using ConcertTicketAPI.Models;
using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventService _eventService;

    public EventsController(ILogger<EventsController> logger, IEventService eventService)
    {
        _logger = logger;
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error retrieving events";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var ev = await _eventService.GetByIdAsync(id);
            return ev == null ? NotFound() : Ok(ev);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error retrieving event by ID";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }
}
