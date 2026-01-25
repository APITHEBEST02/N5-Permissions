using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Querys.GetPermissions;
public class GetPermissionsQuery : IRequest<IEnumerable<PermissionDto>>
{
    
}