# ConcertTicketAPI
This repository contains C# projects for managing concert tickets and events

Core features:
* Event Management
* Ticket Reservations and Sales
* Venue Capacity Management

Domain Features:
* Events
  + Create/update concert events
  + Set ticket types and pricing
  + Manage available capacity
  + Basic event details (date, venue, description)

* Tickets
  + Reserve tickets for a time window
  + Purchase tickets (payment processing is stubbed)
  + Cancel reservations
  + View ticket availability

The `ConcertTicketAPI` project contains a .Net Web API with endpoints for managing concert tickets and events

The `ConcertTicketAPI.Tests` project contains unit and integration tests for the `ConcertTicketAPI`.

Folder Structure
```
ConcertTicketAPI/
|--Controllers
|--Models
|--DTOs
|--Services
|--Repositories
ConcertTicketAPI.Test/
|--Controllers
|--Services
|--Integration
```
