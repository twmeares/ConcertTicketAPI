using ConcertTicketAPI.DTOs;

namespace ConcertTicketAPI.Services;

public interface ITicketService
{
    Task<List<TicketResponse>> GetAvailableTicketsByEventIdAsync(Guid eventId);
    Task<TicketTransactionResponse> ReserveTicketsAsync(TicketRequest ticketRequest);
    Task<TicketTransactionResponse> CancelReservationAsync(TicketRequest ticketRequest);
    Task<TicketTransactionResponse> PurchaseTicketsAsync(TicketRequest ticketRequest);
    Task<List<TicketResponse>> CreateTicketsAsync(List<CreateTicketRequest> createTicketRequests);
    
}