using TicketingSolution.Core.Domain;

namespace TicketingSolution.Core.DataServices
{
    public interface ITicketBookingService
    {
        void Save(TicketBooking ticketBooking);
    }
}
