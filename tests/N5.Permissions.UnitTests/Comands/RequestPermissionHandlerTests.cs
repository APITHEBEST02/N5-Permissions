using Xunit;
using Moq;
using FluentAssertions;
using N5.Permissions.Application.Comands.RequestPermission;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Repositories;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.UnitTests.Comands;

public class RequestPermissionHandlerTests
{

    // TEST 1: Verificar que se crea el permiso correctamente
    [Fact]
    public async Task Handle_shoudCreatePermission_WhenCommandIsValid()
    {
       
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockPermissionRepo = new Mock<IPermissionRepository>();
        var mockElasticsearch = new Mock<IElasticsearchService>();
        var mockKafka = new Mock<IKafkaProducer>();

         // 2. Configurar el mock del repositorio para que retorne un permiso con ID
        var permissionToReturn = new Permission 
        { 
            Id = 1,
            NombreEmpleado = "Juan",
            ApellidoEmpleado = "Pérez",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };

         mockPermissionRepo
            .Setup(x => x.AddAsync(It.IsAny<Permission>())) 
            .ReturnsAsync(permissionToReturn);
        
         mockUnitOfWork
            .Setup(x => x.Permissions)
            .Returns(mockPermissionRepo.Object);

        mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); 
        
         var handler = new RequestPermissionHandler(
            mockUnitOfWork.Object,
            mockElasticsearch.Object,
            mockKafka.Object
        );

        var command = new RequestPermissionCommand
        {
            NombreEmpleado = "Juan",
            ApellidoEmpleado = "Pérez",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().Be(1);

        mockPermissionRepo.Verify(
            x => x.AddAsync(It.Is<Permission>(p => 
                p.NombreEmpleado == "Juan" && 
                p.ApellidoEmpleado == "Pérez"
            )), 
            Times.Once
        );

        mockUnitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Once
        );

         mockElasticsearch.Verify(
            x => x.IndexPermissionAsync(It.IsAny<Permission>()), 
            Times.Once
        );

         mockKafka.Verify(
            x => x.SendAsync(It.Is<OperationMessage>(m => 
                m.NameOperation == "request"
            )), 
            Times.Once
        );
        
    }


    // TEST 2: Verificar que falla si hay error en DB
    [Fact]
    public async Task Handle_ShouldThrowException_WhenDatabaseFails()
    {

        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockPermissionRepo = new Mock<IPermissionRepository>();
        var mockElasticsearch = new Mock<IElasticsearchService>();
        var mockKafka = new Mock<IKafkaProducer>();
        
        
        mockPermissionRepo
            .Setup(x => x.AddAsync(It.IsAny<Permission>()))
            .ThrowsAsync(new Exception("Database error"));
        
        mockUnitOfWork
            .Setup(x => x.Permissions)
            .Returns(mockPermissionRepo.Object);
        
        var handler = new RequestPermissionHandler(
            mockUnitOfWork.Object,
            mockElasticsearch.Object,
            mockKafka.Object
        );
        
        var command = new RequestPermissionCommand
        {
            NombreEmpleado = "Juan",
            ApellidoEmpleado = "Pérez",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };
        
        
        await Assert.ThrowsAsync<Exception>(() => 
            handler.Handle(command, CancellationToken.None)
        );
    }

}