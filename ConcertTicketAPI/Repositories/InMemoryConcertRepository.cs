using System;
using ConcertTicketAPI.DTOs;
using ConcertTicketAPI.Models;

namespace ConcertTicketAPI.Repositories;

/// <summary>
/// In-memory implementation of the concert repository for testing purposes.
/// This class simulates a data store for events and tickets without using a database.
/// </summary>
public class InMemoryConcertRepository : IConcertRepository
{
    private readonly List<Event> _events;
    private readonly List<Ticket> _tickets;

    public InMemoryConcertRepository()
    {
        _events = new List<Event>();
        _tickets = new List<Ticket>();
    }

    public Task AddEventAsync(Event ev)
    {
        _events.Add(ev);
        return Task.CompletedTask;
    }

    public Task<Event?> UpdateEventAsync(Event ev)
    {
        var existingEvent = _events.FirstOrDefault(e => e.Id == ev.Id);
        if (existingEvent != null)
        {
            //replace existing with updated event
            _events[_events.IndexOf(existingEvent)] = ev;
            return Task.FromResult<Event?>(ev);
        }
        return Task.FromResult<Event?>(null);
    }

    public Task<Event?> GetEventByIdAsync(Guid id)
    {
        var ev = _events.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(ev);
    }

    public Task<List<Event>> GetAllEventsAsync()
    {
        return Task.FromResult(_events);
    }

    public Task<List<Ticket>> GetAvailableTicketsByEventIdAsync(Guid eventId)
    {
        var tickets = _tickets.Where(t => t.EventId == eventId && t.IsAvailable).ToList();
        return Task.FromResult(tickets);
    }
    
    public Task<List<Ticket>> GetTicketsByTicketIdsAsync(List<Guid> ticketIds)
    {
        var tickets = _tickets.Where(t => ticketIds.Contains(t.Id)).ToList();
        return Task.FromResult(tickets);
    }

    public Task<bool> ReserveTicketsAsync(Guid userId, List<Guid> ticketIds)
    {
        // attempt to find the tickets based on ticket id in request list
        var ticketsToReserve = _tickets.Where(t => ticketIds.Contains(t.Id) && t.IsAvailable).ToList();

        // if number of tickets returned in ticketsToReserve doesn't equal number of tickets requested return false
        if (ticketsToReserve.Count != ticketIds.Count)
        {
            // NOTE: in a real db, this would be a transaction that would fail if the tickets were not available
            return Task.FromResult(false);
        }
        else
        {
            //update tickets to reserved state
            foreach (var ticket in ticketsToReserve)
            {
                ticket.UserId = userId;
                ticket.ReservedUntil = DateTime.UtcNow.AddMinutes(15); // reserve for 15 minutes, would make this configurable in a real app
            }
        }
        return Task.FromResult(true); // return true if all tickets were reserved successfully
    }

    /// <summary>
    /// marks the given tickets as available by clearing the userId and reservation/purchase dates.
    /// If onlyMarkNonPurchased is true, it will only mark tickets that have not been purchased.
    /// If false, it will mark all tickets as available regardless of purchase status.
    /// </summary>
    public Task<bool> MarkAvailableAsync(Guid userId, List<Guid> ticketIds, bool onlyMarkNonPurchased)
    {
        // attempt to find tickets that are still reserved by the user and haven't been purchased
        var ticketsToReserve = _tickets.Where(t => ticketIds.Contains(t.Id) && t.UserId == userId).ToList();

        if (onlyMarkNonPurchased)
        {
            // if only marking non-purchased tickets, filter out those that have been purchased
            ticketsToReserve = ticketsToReserve.Where(t => t.PurchaseDate > DateTime.Now).ToList();
        }

        // if number of tickets returned in ticketsToReserve doesn't equal number of tickets requested return false
        // this is to prevent cancelling a reservation that has already been purchased
        if (ticketsToReserve.Count != ticketIds.Count)
        {
            // NOTE: in a real db, this would be a transaction that would fail if the tickets were not available
            return Task.FromResult(false);
        }
        else
        {
            // remove reservation for each ticket
            foreach (var ticket in ticketsToReserve)
            {
                ticket.UserId = Guid.Empty; // clear userId to indicate no reservation;
                ticket.ReservedUntil = DateTime.MinValue;
                ticket.PurchaseDate = DateTime.MaxValue;
            }
        }
        return Task.FromResult(true); // return true if all reservations were cancelled successfully
    }

    public Task<bool> CancelReservationAsync(Guid userId, List<Guid> ticketIds)
    {
        var onlyMarkNonPurchased = true;
        return MarkAvailableAsync(userId, ticketIds, onlyMarkNonPurchased);
    }

    public Task<bool> CancelPurchaseAsync(Guid userId, List<Guid> ticketIds)
    {
        var onlyMarkNonPurchased = false;
        return MarkAvailableAsync(userId, ticketIds, onlyMarkNonPurchased);
    }

    public Task<bool> PurchaseTicketsAsync(Guid userId, List<Guid> ticketIds)
    {
        // attempt to find tickets that are still reserved by the user and haven't been purchased
        var reservedTickets = _tickets
            .Where(t => ticketIds.Contains(t.Id) && t.UserId == userId && t.PurchaseDate < DateTime.Now && t.ReservedUntil > DateTime.UtcNow)
            .ToList();

        // if number of tickets returned in reservedTickets doesn't equal the number of tickets requested return false
        // this is to prevent purchasing tickets that have already been purchased
        if (reservedTickets.Count != ticketIds.Count)
        {
            // NOTE: in a real db, this would be a transaction that would fail if the tickets were not available
            return Task.FromResult(false);
        }
        else
        {
            // remove reservation for each ticket
            foreach (var ticket in reservedTickets)
            {
                ticket.PurchaseDate = DateTime.UtcNow; // set purchase date to now
            }
        }
        return Task.FromResult(true); // return true if all reservations were cancelled successfully
    }

}
