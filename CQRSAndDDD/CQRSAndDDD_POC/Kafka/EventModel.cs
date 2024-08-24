namespace CQRSAndDDD_POC.Kafka
{
    public class EventModel
    {
        public string? User { get; set; }
        public string? Description { get; set; }
        public object? Payload { get; set; }
    }
}
