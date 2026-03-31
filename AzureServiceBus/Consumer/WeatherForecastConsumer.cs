
using Azure.Messaging.ServiceBus;
using AzureServiceBus.Domain;
using System.Text.Json;

namespace AzureServiceBus.Consumer
{
    public class WeatherForecastConsumer : BackgroundService
    {
        private readonly ServiceBusProcessor _processor;
        private readonly IServiceScopeFactory _scopeFactory;

        public WeatherForecastConsumer(
            ServiceBusClient client, 
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _processor = client.CreateProcessor(
                topicName: "topic.1",
                subscriptionName: "subscription.1");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += ProcessMessage;
            _processor.ProcessErrorAsync += ProcessError;

            await _processor.StartProcessingAsync(stoppingToken);
        }

        public async Task ProcessMessage(ProcessMessageEventArgs args)
        {
            var json = args.Message.Body.ToString();

            var message = JsonSerializer.Deserialize<MessageQueue>(json);

            Console.WriteLine($"AMessage recebida: {message.nome}");

            if(message != null)
                Console.WriteLine("Processando");

            await args.CompleteMessageAsync(args.Message);
        }

        public Task ProcessError(ProcessErrorEventArgs args)
        {
            Console.WriteLine($"Erro {args.Exception.Message}");

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.StopProcessingAsync(cancellationToken); 

            await base.StopAsync(cancellationToken);
        }
    }
}
