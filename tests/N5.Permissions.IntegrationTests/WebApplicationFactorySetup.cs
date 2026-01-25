using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N5.Permissions.Infrastructure.Persistence;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Interfaces;
using Moq;

namespace N5.Permissions.IntegrationTests;


public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureServices(services =>
        {
            // Remover Kafka y reemplazar con un mock
            var kafkaDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IKafkaProducer));
            if (kafkaDescriptor != null)
            {
                services.Remove(kafkaDescriptor);
            }

            // Agregar un mock de Kafka
            var mockKafka = new Mock<IKafkaProducer>();
            mockKafka.Setup(x => x.SendAsync(It.IsAny<OperationMessage>()))
                     .Returns(Task.CompletedTask);
            services.AddSingleton(mockKafka.Object);
            
            // Remover Elasticsearch y reemplazar con un mock
            var elasticDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IElasticsearchService));
            if (elasticDescriptor != null)
            {
                services.Remove(elasticDescriptor);
            }

            // Agregar un mock de Elasticsearch
            var mockElastic = new Mock<IElasticsearchService>();
            mockElastic.Setup(x => x.IndexPermissionAsync(It.IsAny<Permission>()))
                      .Returns(Task.CompletedTask);
            services.AddSingleton(mockElastic.Object);
        });
    }

    public HttpClient GetAuthenticatedClient()
    {
        var client = CreateClient();
        
        // Seed database
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Seed data
            if (!db.PermissionTypes.Any())
            {
                db.PermissionTypes.Add(new PermissionType
                {
                    Id = 1,
                    Descripcion = "Permiso de prueba"
                });
                db.SaveChanges();
            }
        }

        return client;
    }
}