

using CQRSAndDDD.Domain;

namespace CQRSAndDDD.Infrastrucure.Repos.Boundry
{
    public interface ITicketRepo
    {
        Task AddAsync(Ticket ticket);
        Task<List<Ticket>> GetTicketsAsync(int pageNumber, int pageSize);
        Task UpdateAsync(Ticket ticket);
    }
}
