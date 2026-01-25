using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Permissions.Application.Comands.RequestPermission;
using N5.Permissions.Application.Comands.ModifyPermission;
using N5.Permissions.Application.Querys.GetPermissions;

namespace N5.Permissions.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("request")]
    public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [HttpPut("modify/{id}")]
    public async Task<IActionResult> ModifyPermission(int id, [FromBody] ModifyPermissionCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetPermissions()
    {
        var permissions = await _mediator.Send(new GetPermissionsQuery());
        return Ok(permissions);
    }
}