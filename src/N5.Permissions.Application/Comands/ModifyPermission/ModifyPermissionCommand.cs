using MediatR;

namespace N5.Permissions.Application.Comands.ModifyPermission;

public class ModifyPermissionCommand : IRequest<int>
{
    public int Id { get; set; }
    public string NombreEmpleado { get; set; } = string.Empty;
    public string ApellidoEmpleado { get; set; } = string.Empty;
    public int TipoPermiso { get; set; }
    public DateTime FechaPermiso { get; set; }
}