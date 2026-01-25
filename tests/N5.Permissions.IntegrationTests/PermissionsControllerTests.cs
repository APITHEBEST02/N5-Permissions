using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using N5.Permissions.Application.Comands.RequestPermission;
using N5.Permissions.Application.Comands.ModifyPermission;
using N5.Permissions.Application.DTOs;
using Xunit;

namespace N5.Permissions.IntegrationTests;

public class PermissionsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    
    public PermissionsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.GetAuthenticatedClient();
    }
    
    // TEST 1: Request Permission - Happy Path
    [Fact]
    public async Task RequestPermission_ShouldReturn200_WhenCommandIsValid()
    {
        // ARRANGE
        var command = new RequestPermissionCommand
        {
            NombreEmpleado = "Carlos",
            ApellidoEmpleado = "Rodríguez",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };
        
        // ACT
        var response = await _client.PostAsJsonAsync("/api/permissions/request", command);
        
        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<PermissionCreatedResponse>();
        result.Should().NotBeNull();
        result!.Id.Should().BeGreaterThan(0);
    }
    
    // TEST 2: Get Permissions - Verificar que retorna lista
    [Fact]
    public async Task GetPermissions_ShouldReturnList()
    {
        

        var command = new RequestPermissionCommand
        {
            NombreEmpleado = "Ana",
            ApellidoEmpleado = "López",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };
        await _client.PostAsJsonAsync("/api/permissions/request", command);
        

        var response = await _client.GetAsync("/api/permissions");
        

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var permissions = await response.Content.ReadFromJsonAsync<List<PermissionDto>>();
        permissions.Should().NotBeNull();
        permissions.Should().NotBeEmpty();
        permissions.Should().Contain(p => p.NombreEmpleado == "Ana");
    }
    

    // TEST 3: Modify Permission - Happy Path
    [Fact]
    public async Task ModifyPermission_ShouldReturn204_WhenPermissionExists()
    {
        
        var createCommand = new RequestPermissionCommand
        {
            NombreEmpleado = "Luis",
            ApellidoEmpleado = "Martínez",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };
        var createResponse = await _client.PostAsJsonAsync("/api/permissions/request", createCommand);
        var createdResult = await createResponse.Content.ReadFromJsonAsync<PermissionCreatedResponse>();
        int permissionId = createdResult!.Id;
        
        var modifyCommand = new ModifyPermissionCommand
        {
            Id = permissionId,
            NombreEmpleado = "Luis Modificado",
            ApellidoEmpleado = "Martínez Modificado",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };
        

        var response = await _client.PutAsJsonAsync($"/api/permissions/modify/{permissionId}", modifyCommand);
        

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    

    // TEST 4: Request Permission con datos inválidos
    [Fact(Skip = "FluentValidation not yet configured")]
    public async Task RequestPermission_ShouldReturn400_WhenCommandIsInvalid()
    {

        var command = new RequestPermissionCommand
        {
            NombreEmpleado = "", // Nombre vacío (inválido)
            ApellidoEmpleado = "García",
            TipoPermiso = 1,
            FechaPermiso = DateTime.Now
        };

        var response = await _client.PostAsJsonAsync("/api/permissions/request", command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

// Clase para deserializar la respuesta del RequestPermission
public class PermissionCreatedResponse
{
    public int Id { get; set; }
}