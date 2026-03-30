
using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace AzureServiceBus.Infrastructure.MessageBus
{
    public class AzureBusService : IBusService
    {
        private readonly ServiceBusClient _client;

        public AzureBusService(ServiceBusClient client)
        {
            _client = client;
        }

        public async Task SendMessageAsync<T>(string queueName, T message)
        {
            var sender = _client.CreateSender(queueName);

            var json = JsonSerializer.Serialize(message);

            var serviceBusMessage = new ServiceBusMessage(json)
            {
                ContentType = "application/json"
            };

            try
            {
                await sender.SendMessageAsync(serviceBusMessage);
            }
            catch (ServiceBusException ex)
            {
                Console.WriteLine($"Erro Service Bus: {ex.Reason}");
                Console.WriteLine($"Mensagem: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro genérico: {ex.Message}");
            }
        }
    }
}
