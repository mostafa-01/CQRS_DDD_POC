namespace CQRSAndDDD_POC.Kafka
{
    public interface IProducer
    {
        public Task produceMessage(string topic, EventModel eventModel);
    }
}
