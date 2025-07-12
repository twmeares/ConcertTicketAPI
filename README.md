# ConcertTicketAPI
This repository contains C# projects for managing concert tickets and events

Core features:
* Event Management
* Ticket Reservations and Sales
* Venue Capacity Management

Domain Features:
* Events
  + Create/update concert events
  + Basic event details (date, venue, description)

* Tickets
  + Reserve tickets for a time window
  + Purchase tickets (payment processing is stubbed)
  + Cancel reservations
  + View ticket availability
  + Create tickets

Key Assumptions and tradeoffs
* I've chosen to go for a minimum viable product approach while covering the domain features listed above. 
* I chose to put ticket creation as part of the ticketController and not under the scope of events to help make the APIs map to their specific data objects. Tickets are treated as individual DB items to support per ticket pricing and optional per ticket seat/row info.
* Ticket creation does not currently enforce venue capacity limits. I intentionally left this out, as in a real system I would expect capacity constraints to be enforced at the database level.
* For this assignment I have written minimal test cases to ensure Key functionality, but due to time constraints I've omitted many important test.
* The project uses an in memory implementation for managening what would likely be housed in a database, but I have written it as an interface (IConcertRepository) to allow for easily swapping this in the future.
* The project does not currently implement any authorization or authentication, but in the real world this would need to be done. JWTs could be used for authentication and a user role schema could be created to ensure certain actions like creating tickets/events are only allowed by Admin users.
* I used a DTO (Data Transfer Object) structure to abstract away some of the request/response logic so as not to directly pass the DB model objects back to the client. This allows for easiler updates to the solution if the underlying DB models change or new fields are needed on the request/reponse.
* For the reservation feature I have chosen to use a simple time based reservation of 15 minutes. In production this would need to be configurable and would likely be implemented through a type of distrubuted lock rather than directly in the main DB.
* I've chosen to use a pattern of adding try/catch at the controller level and favored directly throwing exceptions at the service level and below.
* One potential area for improvement is in the responses of the reserve, cancel reservation, and purchase methods. For now I've opted to simply return a boolean success if there is an issue such as only part of the requested ticket list could be reserved. I've added fields to the response DTO to make this possible in the future, but for time sake I've chosen to not implement this now. Knowing which tickets failed to reserve, etc. could be helpful for updating the UI.
* Additional areas for improvements include: adding additional data validation beyond what is provided by C# [ApiController], adding metric capture, additional logging

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
