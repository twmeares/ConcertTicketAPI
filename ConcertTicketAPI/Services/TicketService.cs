using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;
using ConcertTicketAPI.Repositories;
using ConcertTicketAPI.Services;

public class TicketService : ITicketService
{
    private readonly ILogger<TicketService> _logger;
    private readonly IConcertRepository _ticketRepository;

    public TicketService(ILogger<TicketService> logger, IConcertRepository ticketRepository)
    {
        _logger = logger;
        _ticketRepository = ticketRepository;
    }

    public async Task<List<TicketResponse>> GetAvailableTicketsByEventIdAsync(Guid eventId)
    {
        var tickets = await _ticketRepository.GetAvailableTicketsByEventIdAsync(eventId);
        if (tickets == null || tickets.Count == 0)
        {
            return new List<TicketResponse>();
        }

        // map Ticket to TicketResponse
        List<TicketResponse> responses = new List<TicketResponse>();
        foreach (var ticket in tickets)
        {
            responses.Add(new TicketResponse
            {
                Id = ticket.Id,
                EventId = ticket.EventId,
                UserId = ticket.UserId,
                TicketType = ticket.TicketType,
                Price = ticket.Price,
                Row = ticket.Row,
                Seat = ticket.Seat
            });
        }
        return responses;
    }

    public async Task<TicketTransactionResponse> ReserveTicketsAsync(TicketRequest ticketRequest)
    {
        var ticketIds = ticketRequest.ticketIds;
        var userId = ticketRequest.UserId;
        // attempt to find the tickets based on ticket id in request list
        var ticketsToReserve = await _ticketRepository.GetTicketsByTicketIdsAsync(ticketIds);

        // filter tickets to only those that are available
        ticketsToReserve = ticketsToReserve.Where(t => t.IsAvailable).ToList();

        // if number of tickets available doesn't equal number of tickets requested, throw exception
        if (ticketsToReserve.Count != ticketIds.Count)
        {
            _logger.LogWarning($"Some tickets were unavialbe to reserve by user {userId}. Requested tickets {string.Join(", ", ticketIds)}");
            return new TicketTransactionResponse
            {
                Success = false,
                Message = "Failed to reserve because some of the tickets were not available.",
            };
        }

        var success = await _ticketRepository.ReserveTicketsAsync(userId, ticketIds);
        if (!success)
        {
            _logger.LogError($"DB error for reserve tickets by user {userId} for tickets {string.Join(", ", ticketIds)}");
            throw new InvalidOperationException("Failed to reserve tickets.");
        }

        var response = new TicketTransactionResponse
        {
            Success = success,
            ReservedTicketIds = ticketIds,
        };
        return response;
    }

    public async Task<TicketTransactionResponse> CancelReservationAsync(TicketRequest ticketRequest)
    {

        var ticketIds = ticketRequest.ticketIds;
        var userId = ticketRequest.UserId;
        // attempt to cancel reservation for any tickets in the request that are still reserved by the current user
        var success = await _ticketRepository.CancelReservationAsync(userId, ticketIds);

        if (!success)
        {
            _logger.LogError($"DB error for cancel reservation by user {userId} for tickets {string.Join(", ", ticketIds)}");
            throw new InvalidOperationException("Failed to cancel reservation.");
        }

        var response = new TicketTransactionResponse
        {
            Success = success,
            ReservedTicketIds = ticketIds,
        };
        return response;

    }

    public async Task<TicketTransactionResponse> PurchaseTicketsAsync(TicketRequest ticketRequest)
    {
        var ticketIds = ticketRequest.ticketIds;
        var userId = ticketRequest.UserId;
        // attempt to find the tickets based on ticket id in request list
        var ticketsToPurchase = await _ticketRepository.GetTicketsByTicketIdsAsync(ticketIds);


        // filter to find tickets that are still reserved by the user and haven't been purchased
        ticketsToPurchase = ticketsToPurchase
            .Where(t => t.UserId == userId && t.PurchaseDate < DateTime.Now && t.ReservedUntil > DateTime.UtcNow).ToList();

        // if number of tickets available doesn't equal number of tickets requested, throw exception
        if (ticketsToPurchase.Count != ticketIds.Count)
        {
            _logger.LogWarning($"Some tickets were unavialbe for purchase by user {userId}. Requested tickets {string.Join(", ", ticketIds)}");
            return new TicketTransactionResponse
            {
                Success = false,
                Message = "Failed to purchase because some of the tickets were not available.",
            };
        }

        var paymentSuccess = await Call3rdPartyPaymentServiceAsync(ticketRequest);
        if (!paymentSuccess)
        {
            _logger.LogError($"Payment processing failed for user {userId} for tickets {string.Join(", ", ticketIds)}");
            throw new InvalidOperationException("Payment processing failed.");
        }

        // mark tickets as purchased in db
        var success = await _ticketRepository.PurchaseTicketsAsync(userId, ticketIds);
        if (!success)
        {
            _logger.LogError($"DB error for purchase by user {userId} for tickets {string.Join(", ", ticketIds)}");
            throw new InvalidOperationException("Failed to purchase tickets.");
        }

        var response = new TicketTransactionResponse
        {
            Success = success,
            ReservedTicketIds = ticketIds,
        };
        return response;
    }
    
    private async Task<bool> Call3rdPartyPaymentServiceAsync(TicketRequest ticketRequest)
    {
        // Simulate a call to a 3rd party payment service
        // In a real application, this would involve making an HTTP request to a payment service API
        await Task.Delay(500);
        return true;
    }

    public Task<List<TicketResponse>> CreateTicketsAsync(List<CreateTicketRequest> createTicketRequests)
    {
        List<Ticket> tickets = new List<Ticket>();
        foreach (var request in createTicketRequests)
        {
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                EventId = request.EventId,
                UserId = Guid.Empty, // Initially no user assigned
                PurchaseDate = DateTime.MaxValue, // Not purchased yet
                ReservedUntil = DateTime.MinValue, // Not reserved yet
                TicketType = request.TicketType,
                Price = request.Price,
                Row = request.Row,
                Seat = request.Seat
            };
            tickets.Add(ticket);
        }

        _ticketRepository.AddTicketsAsync(tickets);
        
        return Task.FromResult(tickets.Select(t => new TicketResponse
        {
            Id = t.Id,
            EventId = t.EventId,
            UserId = t.UserId,
            TicketType = t.TicketType,
            Price = t.Price,
            Row = t.Row,
            Seat = t.Seat
        }).ToList());
    }
}