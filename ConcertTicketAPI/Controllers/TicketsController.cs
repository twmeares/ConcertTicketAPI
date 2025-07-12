using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ILogger<TicketsController> _logger;
    private readonly ITicketService _ticketService;

    public TicketsController(ILogger<TicketsController> logger, ITicketService ticketService)
    {
        _logger = logger;
        _ticketService = ticketService;
    }

    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetAvailableTicketsByEventId(Guid eventId)
    {
        try
        {
            var tickets = await _ticketService.GetAvailableTicketsByEventIdAsync(eventId);
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error retrieving tickets";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    
}
