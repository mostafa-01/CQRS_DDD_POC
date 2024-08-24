using Confluent.Kafka;
using static CQRSAndDDD_POC.Kafka.EventSerialization;

namespace CQRSAndDDD_POC.Kafka
{
    public class Producer : IProducer
    {
        private readonly IConfiguration _configuration;
        private readonly ProducerConfig _producerConfig;

        public Producer(IConfiguration configuration)
        {
            _configuration = configuration;
            var _config = _configuration.GetSection("producerConfig").Get<ProducerConfiguration>();
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                Acks = (Acks?)Enum.Parse(typeof(Acks), _config.Acks, true),
                MessageSendMaxRetries = int.Parse(_config.MessageSendMaxRetries),
                AllowAutoCreateTopics = true,
                RetryBackoffMs = int.Parse(_config.RetryBackoffMs),
                EnableIdempotence = bool.Parse(_config.EnableIdempotence)
            };
        }

        public async Task produceMessage(string topic, EventModel eventModel)
        {
            //eventModel.User ??= JwtUtil.GetActiveUserNationalId();

            try
            {
                using var producer = new ProducerBuilder<string, EventModel>(_producerConfig)
                                            .SetValueSerializer(new EventSerializer<EventModel>())
                                            .Build();
                var response = await producer.ProduceAsync(topic, new Message<string, EventModel>
                {
                    Value = eventModel,
                    Key = eventModel.User
                });
            }
            catch (Exception ex)
            {
                //throw new ProducerException(topic, ex);
            }
        }
    }
    public class ProducerConfiguration
    {
        public string? BootstrapServers { get; set; }
        public string? Acks { get; set; }
        public string? MessageSendMaxRetries { get; set; }
        public string? RetryBackoffMs { get; set; }
        public string? EnableIdempotence { get; set; }

        public ProducerConfiguration() { }
    }
}
