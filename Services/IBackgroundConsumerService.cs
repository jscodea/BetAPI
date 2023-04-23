namespace BetAPI.Services
{
    public interface IBackgroundConsumerService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
