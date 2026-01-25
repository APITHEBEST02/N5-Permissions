using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using N5.Permissions.Domain.Interfaces;
using System.Text.Json;

namespace N5.Permissions.Infrastructure.Services;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducer(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task SendAsync(OperationMessage message)
    {
        var json = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync("permissions-topic", 
            new Message<string, string>
            {
                Key = message.Id.ToString(),
                Value = json
            });
    }
}