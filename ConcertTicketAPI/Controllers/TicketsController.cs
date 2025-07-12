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
            if (tickets == null || !tickets.Any())
            {
                _logger.LogInformation($"No available tickets found for event {eventId}");
                return NotFound("No available tickets found for this event.");
            }
            return Ok(tickets);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error retrieving tickets";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveTickets([FromBody] TicketRequest ticketRequest)
    {
        if (ticketRequest == null || !ticketRequest.TicketIds.Any() || ticketRequest.UserId == Guid.Empty)
        {
            _logger.LogWarning("Invalid ticket request received.");
            return BadRequest("Invalid ticket request.");
        }

        try
        {
            var response = await _ticketService.ReserveTicketsAsync(ticketRequest);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error reserving tickets";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseTickets([FromBody] TicketRequest ticketRequest)
    {
        if (ticketRequest == null || !ticketRequest.TicketIds.Any() || ticketRequest.UserId == Guid.Empty)
        {
            _logger.LogWarning("Invalid ticket request received.");
            return BadRequest("Invalid ticket request.");
        }

        try
        {
            var response = await _ticketService.PurchaseTicketsAsync(ticketRequest);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error purchasing tickets";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    [HttpPost("cancel-reservation")]
    public async Task<IActionResult> CancelReservation([FromBody] TicketRequest ticketRequest)
    {
        if (ticketRequest == null || !ticketRequest.TicketIds.Any() || ticketRequest.UserId == Guid.Empty)
        {
            _logger.LogWarning("Invalid ticket request received.");
            return BadRequest("Invalid ticket request.");
        }

        try
        {
            var response = await _ticketService.CancelReservationAsync(ticketRequest);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error canceling reservation";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }

    [HttpPost("create-tickets")]
    public async Task<IActionResult> CreateTickets([FromBody] List<CreateTicketRequest> tickets)
    {
        if (tickets == null || !tickets.Any())
        {
            _logger.LogWarning("Invalid ticket creation request received.");
            return BadRequest("Invalid ticket creation request.");
        }

        try
        {
            var createdTickets = await _ticketService.CreateTicketsAsync(tickets);
            return CreatedAtAction(nameof(CreateTickets), new { eventId = tickets.First().EventId }, createdTickets);
        }
        catch (Exception ex)
        {
            var errorMsg = "Error creating tickets";
            _logger.LogError(ex, errorMsg);
            return StatusCode(500, errorMsg);
        }
    }
}
