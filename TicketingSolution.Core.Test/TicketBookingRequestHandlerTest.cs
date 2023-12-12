using Moq;
using Shouldly;
using System.Net.Sockets;
using TicketingSolution.Core.DataServices;
using TicketingSolution.Core.Domain;
using TicketingSolution.Core.Enums;
using TicketingSolution.Core.Handlers;

namespace TicketingSolution.Core.Test
{
    public class TicketBookingRequestHandlerTest
    {
        private readonly Mock<ITicketBookingService> _ticketBookingServiceMock;
        private readonly TicketBookingRequestHandler _handler;
        private readonly TicketBookingRequest _bookingRequest;
        private List<Ticket> _availableTickets;

        public TicketBookingRequestHandlerTest()
        {
            //Arrange

            _bookingRequest = new TicketBookingRequest
            {
                Name = "Test Name",
                Family = "Test Family",
                Email = "Test Email",
                Date = DateTime.Now,
            };

            _availableTickets = new List<Ticket>() { new Ticket() { Id = 1 } };
            _ticketBookingServiceMock = new Mock<ITicketBookingService>();
            _ticketBookingServiceMock.Setup(q => q.GetAvailableTickets(_bookingRequest.Date))
                .Returns(_availableTickets);

            _handler = new TicketBookingRequestHandler(_ticketBookingServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnTicketBookingResponseWithRequestValues()
        {
            //Act
            TicketBookingResult Result = _handler.ServiceBooking(_bookingRequest);

            //Assert
            //Assert.NotNull(Result);
            //Assert.Equal(Result.Name, BookingRequest.Name);
            //Assert.Equal(Result.Family, BookingRequest.Family);
            //Assert.Equal(Result.Email, BookingRequest.Email);

            //Assert By Shouldly
            Result.ShouldNotBeNull();
            Result.Name.ShouldBe(_bookingRequest.Name);
            Result.Family.ShouldBe(_bookingRequest.Family);
            Result.Email.ShouldBe(_bookingRequest.Email);
        }

        [Fact]
        public void ShouldThrowExceptionForNullRequest()
        {
            var exception = Should.Throw<ArgumentNullException>(() => _handler.ServiceBooking(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void ShouldSaveTicketBookingRequest()
        {
            TicketBooking savedBooking = null;
            var result = _ticketBookingServiceMock.Setup(x => x.Save(It.IsAny<TicketBooking>()))
                    .Callback<TicketBooking>(booking =>
                    {
                        savedBooking = booking;
                    });
            _handler.ServiceBooking(_bookingRequest);
            _ticketBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Once);

            savedBooking.ShouldNotBeNull();
            savedBooking.Name.ShouldBe(_bookingRequest.Name);
            savedBooking.Family.ShouldBe(_bookingRequest.Family);
            savedBooking.Email.ShouldBe(_bookingRequest.Email);
            savedBooking.TicketId.ShouldBe(_availableTickets.First().Id);
        }

        [Fact]
        public void ShouldNotSaveTicketBookingRequestIfNoneAvailable()
        {
            _availableTickets.Clear();
            _handler.ServiceBooking(_bookingRequest);
            _ticketBookingServiceMock.Verify(x => x.Save(It.IsAny<TicketBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure,false)]
        [InlineData(BookingResultFlag.Success,true)]
        public void ShouldReturnSuccessOrFailureFlagInResult(BookingResultFlag bookingSuccessFlag,bool isAvailable)
        {
            if(!isAvailable)
            {
                _availableTickets.Clear();
            }
            var result = _handler.ServiceBooking(_bookingRequest);
            bookingSuccessFlag.ShouldBe(result.Flag);
        }
    }
}