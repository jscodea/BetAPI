using Confluent.Kafka;

namespace BetAPI.Services
{
    public class BackgroundResultsConsumerService : IBackgroundConsumerService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly string topic = "results";
        private readonly string groupId = "local";
        private readonly string bootstrapServers = "localhost:29092";

        public BackgroundResultsConsumerService(ILogger<BackgroundResultsConsumerService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build();
            consumerBuilder.Subscribe(topic);

            _logger.LogInformation(
                    "Consumer started");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumer = consumerBuilder.Consume(stoppingToken);
                _logger.LogInformation(
                    "msg value: {Message}", consumer.Message.Value);

                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
