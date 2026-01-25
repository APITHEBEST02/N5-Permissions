using Xunit;
using Moq;
using FluentAssertions;
using N5.Permissions.Application.Querys.GetPermissions;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Repositories;

namespace N5.Permissions.UnitTests.Queries;

public class GetPermissionsHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnAllPermissions()
    {
        // ARRANGE
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockPermissionRepo = new Mock<IPermissionRepository>();
        
        var permissions = new List<Permission>
        {
            new Permission { Id = 1, NombreEmpleado = "Juan", ApellidoEmpleado = "Pérez", TipoPermiso = 1, FechaPermiso = DateTime.Now },
            new Permission { Id = 2, NombreEmpleado = "María", ApellidoEmpleado = "García", TipoPermiso = 2, FechaPermiso = DateTime.Now }
        };
        
        mockPermissionRepo
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(permissions);
        
        mockUnitOfWork
            .Setup(x => x.Permissions)
            .Returns(mockPermissionRepo.Object);
        
        var handler = new GetPermissionsHandler(mockUnitOfWork.Object);
        
        var query = new GetPermissionsQuery();
        
        // ACT
        var result = await handler.Handle(query, CancellationToken.None);
        
        // ASSERT
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.NombreEmpleado == "Juan");
        result.Should().Contain(p => p.NombreEmpleado == "María");
        
        mockPermissionRepo.Verify(x => x.GetAllAsync(), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoPermissionsExist()
    {
        // ARRANGE
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockPermissionRepo = new Mock<IPermissionRepository>();
        
        mockPermissionRepo
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(new List<Permission>());
        
        mockUnitOfWork
            .Setup(x => x.Permissions)
            .Returns(mockPermissionRepo.Object);
        
        var handler = new GetPermissionsHandler(mockUnitOfWork.Object);
        
        var query = new GetPermissionsQuery();
        
        // ACT
        var result = await handler.Handle(query, CancellationToken.None);
        
        // ASSERT
        result.Should().BeEmpty();
    }
}