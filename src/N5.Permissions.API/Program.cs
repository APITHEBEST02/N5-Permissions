using Microsoft.EntityFrameworkCore;
using N5.Permissions.Infrastructure.Persistence;
using N5.Permissions.Domain.Repositories;
using N5.Permissions.Infrastructure.Repositories;
using N5.Permissions.Application.Comands.RequestPermission;
using Nest;
using N5.Permissions.Domain.Interfaces;
using N5.Permissions.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
// DbContext - Solo usar SqlServer si NO estamos en Testing
if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestDatabase"));
}

// Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RequestPermissionCommand).Assembly));

// Elasticsearch - Solo en producción
if (builder.Environment.IsProduction())
{
    var elasticSettings = new ConnectionSettings(new Uri(builder.Configuration["Elasticsearch:Uri"] ?? ""))
        .DefaultIndex("permissions");
    builder.Services.AddSingleton<IElasticClient>(new ElasticClient(elasticSettings));
    builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();
}
else
{
    // Mock de Elasticsearch en desarrollo
    builder.Services.AddScoped<IElasticsearchService>(sp => 
    {
        var mock = new Moq.Mock<IElasticsearchService>();
        mock.Setup(x => x.IndexPermissionAsync(Moq.It.IsAny<N5.Permissions.Domain.Entities.Permission>()))
            .Returns(Task.CompletedTask);
        return mock.Object;
    });
}

// Kafka - Solo en producción
if (builder.Environment.IsProduction())
{
    builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
}
else
{
    // Mock de Kafka en desarrollo
    builder.Services.AddSingleton<IKafkaProducer>(sp =>
    {
        var mock = new Moq.Mock<IKafkaProducer>();
        mock.Setup(x => x.SendAsync(Moq.It.IsAny<N5.Permissions.Domain.Interfaces.OperationMessage>()))
            .Returns(Task.CompletedTask);
        return mock.Object;
    });
}

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


