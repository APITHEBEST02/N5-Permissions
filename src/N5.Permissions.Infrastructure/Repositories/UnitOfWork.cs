using N5.Permissions.Domain.Repositories;
using N5.Permissions.Infrastructure.Persistence;

namespace N5.Permissions.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IPermissionRepository? _permissions;
    private IPermissionTypeRepository? _permissionTypes;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IPermissionRepository Permissions => 
        _permissions ??= new PermissionRepository(_context);

    public IPermissionTypeRepository PermissionTypes =>
        _permissionTypes ??= new PermissionTypeRepository(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}