

namespace CQRSAndDDD.App.Commands
{
    public  class CreateTicketCommand
    {
        public string PhoneNumber { get; set; }
        public string Governorate { get; set; }
        public string City { get; set; }
        public string District { get; set; }

        public CreateTicketCommand(string phoneNumber, string governorate, string city, string district)
        {
            PhoneNumber = phoneNumber;
            Governorate = governorate;
            City = city;
            District = district;
        }
    }
}
