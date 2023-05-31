using BetAPI.DTO;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace BetAPI.Services
{
    public class BackgroundResultsConsumerService : IBackgroundConsumerService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private readonly IBetService _betService;
        private readonly IEventService _eventService;
        private readonly string topic = "results";
        private readonly string groupId = "local";
        private readonly string bootstrapServers = "localhost:29092";

        public BackgroundResultsConsumerService(ILogger<BackgroundResultsConsumerService> logger, IBetService betService, IEventService eventService)
        {
            _logger = logger;
            _betService = betService;
            _eventService = eventService;
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

            _logger.LogInformation("Consumer started");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumer = consumerBuilder.Consume(stoppingToken);
                _logger.LogInformation(
                    "msg value: {Message}", consumer.Message.Value);

                EventDTO eventDTO = JsonConvert.DeserializeObject<EventDTO>(consumer.Message.Value);
                int eventResultTaskId = await _eventService.SetEventResult(eventDTO.Id, eventDTO.Id);
                if (eventResultTaskId > 0 && eventDTO.Result != null)
                {
                    _betService.BetSettleForEventAsync(eventDTO.Id, (int)eventDTO.Result);
                }

                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
