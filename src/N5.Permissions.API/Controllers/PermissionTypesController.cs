using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Permissions.Application.Comands.CreatePermissionType;
using N5.Permissions.Application.Querys.GetPermissionTypes;

namespace N5.Permissions.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissionTypes()
    {
        var permissionTypes = await _mediator.Send(new GetPermissionTypesQuery());
        return Ok(permissionTypes);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePermissionType([FromBody] CreatePermissionTypeCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }
}
