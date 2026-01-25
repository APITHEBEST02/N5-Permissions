using MediatR;

namespace N5.Permissions.Application.Comands.CreatePermissionType;

public class CreatePermissionTypeCommand : IRequest<int>
{
    public string Descripcion { get; set; } = string.Empty;
}
