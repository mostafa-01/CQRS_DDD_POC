using CQRSAndDDD.App.Queries;
using CQRSAndDDD.Domain;
using CQRSAndDDD.Infrastrucure.Repos.Boundry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CQRSAndDDD.Infrastrucure.Repos.Service.TicketRepoImpl;

namespace CQRSAndDDD.App.Handlers
{
    public class GetTicketsQueryHandler
    {

        private readonly ITicketRepo _ticketRepository;

        public GetTicketsQueryHandler(ITicketRepo ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<List<TicketDTO>> Handle(GetTicketsQuery query)
        {
            if (query.PageNumber < 1)
                    query.PageNumber = 1;
            if (query.PageSize < 1) query.PageSize = 10;

            // Fetch tickets from the repository
            var tickets = await _ticketRepository.GetTicketsAsync(query.PageNumber, query.PageSize);

            return MapTicketsToDtos(tickets);
        }

        private List<TicketDTO> MapTicketsToDtos(List<Ticket> tickets)
        {
            return tickets.Select(ticket => new TicketDTO
            {
                Id = ticket.Id,
                PhoneNumber = ticket.PhoneNumber,
                CreationDateTime = ticket.CreationDateTime,
                Governorate = ticket.Governorate,
                City = ticket.City,
                District = ticket.District,
                IsHandled = ticket.IsHandled,
                Color = ticket.Color // Assuming Color is calculated in the Ticket entity
            }).ToList();
        }
    }
}
