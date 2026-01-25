using MediatR;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Repositories;

namespace N5.Permissions.Application.Comands.CreatePermissionType;

public class CreatePermissionTypeHandler : IRequestHandler<CreatePermissionTypeCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionTypeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreatePermissionTypeCommand request, CancellationToken cancellationToken)
    {
        var permissionType = new PermissionType
        {
            Descripcion = request.Descripcion
        };

        await _unitOfWork.PermissionTypes.AddAsync(permissionType);
        await _unitOfWork.SaveChangesAsync();

        return permissionType.Id;
    }
}
