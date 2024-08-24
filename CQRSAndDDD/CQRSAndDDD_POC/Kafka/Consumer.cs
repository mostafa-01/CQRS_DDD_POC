using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using static CQRSAndDDD_POC.Kafka.EventSerialization;
using CQRSAndDDD_POC.Common;

namespace CQRSAndDDD_POC.Kafka
{
    public class Consumer : BackgroundService
    {

        private readonly List<string> _topics = new() { Constatns.TEST_TOPIC};
        private readonly ConsumerConfiguration _consumerConfig;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IConsumer<string, EventModel?> KafkaConsumer;

        public Consumer(IOptions<ConsumerConfiguration> consumerConfiguration, IServiceScopeFactory scopeFactory)
        {
            _consumerConfig = consumerConfiguration.Value;
            this.scopeFactory = scopeFactory;
            KafkaConsumer = new ConsumerBuilder<string, EventModel?>(_consumerConfig.getConsuerConfig())
                    .SetValueDeserializer(new EventDeserializer<EventModel>())
                    .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
        }
        public void StartConsumerLoop(CancellationToken cancellationToken)
        {
            KafkaConsumer.Subscribe(_topics);

            while (!cancellationToken.IsCancellationRequested)
            {
                var consumedTopic = "";
                try
                {
                    var consumeResult = ConsumeAndCreateTopic(cancellationToken);

                    if (consumeResult is null)
                        break;

                    consumedTopic = consumeResult.Topic;
                    EventModel? eventModel = consumeResult.Message.Value;
                    if (consumeResult.Message.Value is not null)
                    {
                        ConsumeEvent(consumedTopic, consumeResult.Message.Value);

                    }
                }
                catch (Exception ex)
                {

                   // throw new ConsumerException(consumedTopic, ex);
                }
            }
        }

        public void CreateTopic(string Topic)
        {
            using (var adminClient = new AdminClientBuilder(_consumerConfig.getConsuerConfig()).Build())
            {
                adminClient.CreateTopicsAsync(
                    new TopicSpecification[] {
                        new TopicSpecification { Name = Topic, NumPartitions = int.Parse(_consumerConfig.NewTopicNumPartitions), ReplicationFactor = short.Parse(_consumerConfig.NewTopicReplicationFactor) }
                            }).Wait();
            }

            Log.Logger.Warning("Consumer created topic " + Topic + " on kafka broker " + _consumerConfig.BootstrapServers);
        }

        public ConsumeResult<string, EventModel?>? ConsumeAndCreateTopic(CancellationToken stoppingToken)
        {
            try
            {
                return KafkaConsumer.Consume(stoppingToken);
            }
            catch (ConsumeException ex)
            {
                CreateTopic(ex.ConsumerRecord.Topic);
                return null;
            }
        }

        public void ConsumeEvent(string Topic, EventModel eventModel)
        {

            using (var scope = scopeFactory.CreateScope())
            {
                switch (Topic)
                {
                    case "TEST_TOPIC":

                    default:
                        break;
                }
            }
        }

    }
    public class ConsumerConfiguration
    {
        public string? BootstrapServers { get; set; }
        public string? GroupId { get; set; }
        public string AutoOffsetReset { get; set; } = "Earliest";
        public string NewTopicNumPartitions { get; set; } = "1";
        public string NewTopicReplicationFactor { get; set; } = "1";

        public ConsumerConfig getConsuerConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = BootstrapServers,
                GroupId = GroupId,
                AutoOffsetReset = (AutoOffsetReset?)Enum.Parse(typeof(AutoOffsetReset), AutoOffsetReset),
                AllowAutoCreateTopics = true,
                EnableAutoCommit = true
            };
        }

    }
}
