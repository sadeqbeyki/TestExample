using TicketingSolution.Core.Enums;
using TicketingSolution.Core.Models;

namespace TicketingSolution.Core;

public class TicketBookingResult : TicketBookingBase
{
    public BookingResultFlag Flag { get; set; }
}