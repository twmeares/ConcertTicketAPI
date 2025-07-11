using ConcertTicketAPI.DTOs;
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

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] EventRequest request)
    {
        if (request == null)
        {
            return BadRequest("Invalid event data");
        }

        try
        {
            var createdEvent = await _eventService.CreateEvent(request);
            return CreatedAtAction(nameof(CreateEvent), new { id = createdEvent.Id }, createdEvent);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error creating event";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventRequest request)
    {
        if (request == null || id != request.Id)
        {
            return BadRequest("Invalid event data");
        }

        try
        {
            var updated = await _eventService.UpdateEvent(request);
            return updated != null ? Ok(updated) : NotFound();
        }
        catch (Exception ex)
        {
            var errorMsg = "Error updating event";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    // NOTE: skipping patch implementation due to time constraints
}
