

using CQRSAndDDD.App.Commands;
using CQRSAndDDD.Domain;
using CQRSAndDDD.Infrastrucure.Repos.Boundry;

namespace CQRSAndDDD.App.Handlers
{
    public class CreateTicketCommandHandler
    {
        private readonly ITicketRepo _ticketRepository;

        public CreateTicketCommandHandler(ITicketRepo ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Guid> Handle(CreateTicketCommand command)
        {
            var ticket = new Ticket(command.PhoneNumber, command.Governorate, command.City, command.District);
            await _ticketRepository.AddAsync(ticket);
            return ticket.Id;
        }
    }
}
