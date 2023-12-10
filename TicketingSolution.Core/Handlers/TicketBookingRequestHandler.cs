using TicketingSolution.Core;
using TicketingSolution.Core.DataServices;

namespace TicketingSolution.Core.Handlers;

public class TicketBookingRequestHandler
{
    public TicketBookingRequestHandler(ITicketBookingService @object)
    {
    }

    public ServiceBookingResult ServiceBooking(TicketBookingRequest bookingRequest)
    {
        if(bookingRequest is null)
        {
            throw new ArgumentNullException(nameof(bookingRequest));
        }
        return new ServiceBookingResult
        {
            Name = bookingRequest.Name,
            Family = bookingRequest.Family,
            Email = bookingRequest.Email,
        };
    }
}