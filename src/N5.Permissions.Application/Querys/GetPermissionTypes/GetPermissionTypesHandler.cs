using MediatR;
using N5.Permissions.Application.DTOs;
using N5.Permissions.Domain.Repositories;

namespace N5.Permissions.Application.Querys.GetPermissionTypes;

public class GetPermissionTypesHandler : IRequestHandler<GetPermissionTypesQuery, IEnumerable<PermissionTypeDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPermissionTypesHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PermissionTypeDto>> Handle(GetPermissionTypesQuery request, CancellationToken cancellationToken)
    {
        var permissionTypes = await _unitOfWork.PermissionTypes.GetAllAsync();
        
        return permissionTypes.Select(pt => new PermissionTypeDto
        {
            Id = pt.Id,
            Descripcion = pt.Descripcion
        });
    }
}
