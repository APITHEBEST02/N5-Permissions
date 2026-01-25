using MediatR;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Domain.Repositories;

namespace N5.Permissions.Application.Querys.GetPermissions;

public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, IEnumerable<PermissionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPermissionsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await _unitOfWork.Permissions.GetAllAsync();

        return permissions.Select(p => new PermissionDto
        {
            Id = p.Id,
            NombreEmpleado = p.NombreEmpleado,
            ApellidoEmpleado = p.ApellidoEmpleado,
            TipoPermiso = p.TipoPermiso,
            FechaPermiso = p.FechaPermiso
        });
    }
}