using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Domain;
using TicketingSolution.Core.Models;

namespace TicketingSolution.Core.Handlers;

public class TicketBookingRequestHandler
{
    private readonly ITicketBookingService _ticketBookingService;

    public TicketBookingRequestHandler(ITicketBookingService ticketBookingService)
    {
        _ticketBookingService = ticketBookingService;
    }

    public TicketBookingResult ServiceBooking(TicketBookingRequest bookingRequest)
    {
        if (bookingRequest is null)  
        {
            throw new ArgumentNullException(nameof(bookingRequest));
        }

        var availableTickets = _ticketBookingService.GetAvailableTickets(bookingRequest.Date);

        if (availableTickets.Any())
        {
            var ticket = availableTickets.First();
            var ticketBooking = CreateTicketBookingObject<TicketBooking>(bookingRequest);
            ticketBooking.TicketId = ticket.Id;
            _ticketBookingService.Save(ticketBooking);
        }

        return CreateTicketBookingObject<TicketBookingResult>(bookingRequest);

    }
    private static TEntity CreateTicketBookingObject<TEntity>(TicketBookingRequest bookingRequest)
        where TEntity : TicketBookingBase, new()
    {
        return new TEntity
        {
            Name = bookingRequest.Name,
            Family = bookingRequest.Family,
            Email = bookingRequest.Email,
        };
    }
}