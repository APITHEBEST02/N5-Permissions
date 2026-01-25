using MediatR;
using N5.Permissions.Application.DTOs;

namespace N5.Permissions.Application.Querys.GetPermissionTypes;

public class GetPermissionTypesQuery : IRequest<IEnumerable<PermissionTypeDto>>
{
}
