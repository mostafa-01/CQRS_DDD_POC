using CQRSAndDDD.App.Commands;
using CQRSAndDDD.App.Handlers;
using CQRSAndDDD.App.Queries;
using CQRSAndDDD.Infrastrucure.Repos.Boundry;
using Microsoft.AspNetCore.Mvc;

namespace CQRSAndDDD_POC.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : Controller
    {
        private readonly CreateTicketCommandHandler _createTicketCommandHandler;
        private readonly GetTicketsQueryHandler _getTicketsQueryHandler;

        private readonly ITicketRepo _repo;

        public TicketController(CreateTicketCommandHandler createTicketCommandHandler)
        {
            _createTicketCommandHandler = createTicketCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand command)
        {
            var ticketId = await _createTicketCommandHandler.Handle(command);
            return Ok(ticketId);
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets(int pageNumber, int pageSize)
        {
            var queryHandler = new GetTicketsQueryHandler(_repo);
            var tickets = await queryHandler.Handle(new GetTicketsQuery(pageNumber, pageSize));
            return Ok(tickets);
        }
    }
}
