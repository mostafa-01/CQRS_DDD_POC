using CQRSAndDDD.Domain;
using CQRSAndDDD.Infrastrucure.Data;
using CQRSAndDDD.Infrastrucure.Repos.Boundry;
using Microsoft.EntityFrameworkCore;
using System;


namespace CQRSAndDDD.Infrastrucure.Repos.Service
{
    public class TicketRepoImpl : ITicketRepo
    {
        private readonly AppDbContext _context;

        public TicketRepoImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TicketDTO>> GetTicketsAsync(int pageNumber, int pageSize)
        {
            var tickets = await _context.Tickets
                .OrderBy(t => t.CreationDateTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return tickets.Select(t => new TicketDTO
            {
                Id = t.Id,
                PhoneNumber = t.PhoneNumber,
                CreationDateTime = t.CreationDateTime,
                Governorate = t.Governorate,
                City = t.City,
                District = t.District,
                IsHandled = t.IsHandled,
                Color = GetColorBasedOnAge(t.CreationDateTime)
            }).ToList();
        }

        private string GetColorBasedOnAge(DateTime creationDateTime)
        {
            var now = DateTime.UtcNow;
            var age = now - creationDateTime;

            if (age.TotalMinutes <= 15) return "Yellow";
            if (age.TotalMinutes <= 30) return "Green";
            if (age.TotalMinutes <= 45) return "Blue";
            if (age.TotalMinutes <= 60) return "Red";
            return "Default";
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public class TicketDTO
        {
            public Guid Id { get; set; }
            public DateTime CreationDateTime { get; set; }
            public string PhoneNumber { get; set; }
            public string Governorate { get; set; }
            public string City { get; set; }
            public string District { get; set; }
            public bool IsHandled { get; set; }
            public string Color { get; set; }
        }
    }
}
