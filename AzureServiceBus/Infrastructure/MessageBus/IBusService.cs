namespace AzureServiceBus.Infrastructure.MessageBus
{
    public interface IBusService
    {
        Task SendMessageAsync<T>(string queueName, T message);
    }
}
