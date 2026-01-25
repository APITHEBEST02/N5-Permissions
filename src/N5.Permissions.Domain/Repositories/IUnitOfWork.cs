namespace N5.Permissions.Domain.Repositories;

public interface IUnitOfWork
{
    IPermissionRepository Permissions { get; }
    IPermissionTypeRepository PermissionTypes { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}