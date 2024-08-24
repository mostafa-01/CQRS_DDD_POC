namespace CQRSAndDDD.Domain
{
    public class Ticket
    {
        public Guid Id { get; private set; }
        public DateTime CreationDateTime { get; private set; }
        public string PhoneNumber { get; private set; }

        //should be lookups
        public string Governorate { get; private set; }
        public string City { get; private set; }
        public string District { get; private set; }

        public bool IsHandled { get; private set; }
        public DateTime? HandledTime { get; private set; }

        public string Color { get; private set; }

        public Ticket(string phoneNumber, string governorate, string city, string district)
        {
            Id = Guid.NewGuid();
            CreationDateTime = DateTime.UtcNow;
            PhoneNumber = phoneNumber;
            Governorate = governorate;
            City = city;
            District = district;
            IsHandled = false;
        }

        public void MarkAsHandled()
        {
            IsHandled = true;
            HandledTime = DateTime.UtcNow;
        }
    }
}