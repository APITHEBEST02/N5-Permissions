using Xunit;
using Moq;
using FluentAssertions;
using N5.Permissions.Application.Comands.ModifyPermission;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Repositories;
using N5.Permissions.Domain.Interfaces;

namespace N5.Permissions.UnitTests.Commands;
public class ModifyPermissionHandlerTests
{
    [Fact]
    public async Task Handle_ShouldUpdatePermission_WhenPermissionExists()
    {
        // ARRANGE
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockPermissionRepo = new Mock<IPermissionRepository>();
        var mockElasticsearch = new Mock<IElasticsearchService>();
        var mockKafka = new Mock<IKafkaProducer>();
        
        // Simular que el permiso existe
        var existingPermission = new Permission 
        { 
            Id = 1,
            NombreEmpleado = "Juan",
            ApellidoEmpleado = "Pérez",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };
        
        mockPermissionRepo
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingPermission);
        
        mockPermissionRepo
            .Setup(x => x.UpdateAsync(It.IsAny<Permission>()))
            .Returns(Task.CompletedTask);
        
        mockUnitOfWork
            .Setup(x => x.Permissions)
            .Returns(mockPermissionRepo.Object);
        
        mockUnitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);
        
        var handler = new ModifyPermissionHandler(
            mockUnitOfWork.Object,
            mockElasticsearch.Object,
            mockKafka.Object
        );
        
        var command = new ModifyPermissionCommand
        {
            Id = 1,
            NombreEmpleado = "Juan Modificado",
            ApellidoEmpleado = "Pérez Modificado",
            TipoPermiso = 2,
            FechaPermiso = DateTime.Now
        };
        
        // ACT
        await handler.Handle(command, CancellationToken.None);
        
        // ASSERT
        mockPermissionRepo.Verify(x => x.GetByIdAsync(1), Times.Once);
        mockPermissionRepo.Verify(x => x.UpdateAsync(It.IsAny<Permission>()), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockKafka.Verify(x => x.SendAsync(It.Is<OperationMessage>(m => m.NameOperation == "modify")), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowException_WhenPermissionDoesNotExist()
    {
        // ARRANGE
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockPermissionRepo = new Mock<IPermissionRepository>();
        var mockElasticsearch = new Mock<IElasticsearchService>();
        var mockKafka = new Mock<IKafkaProducer>();
        
        // Simular que NO existe el permiso
        mockPermissionRepo
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Permission?)null);
        
        mockUnitOfWork
            .Setup(x => x.Permissions)
            .Returns(mockPermissionRepo.Object);
        
        var handler = new ModifyPermissionHandler(
            mockUnitOfWork.Object,
            mockElasticsearch.Object,
            mockKafka.Object
        );
        
        var command = new ModifyPermissionCommand
        {
            Id = 999,
            NombreEmpleado = "Juan",
            ApellidoEmpleado = "Pérez",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };
        
        // ACT & ASSERT
        await Assert.ThrowsAsync<N5.Permissions.Application.Exceptions.NotFoundException>(() => 
            handler.Handle(command, CancellationToken.None)
        );
    }
}