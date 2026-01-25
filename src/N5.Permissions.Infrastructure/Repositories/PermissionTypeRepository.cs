using Microsoft.EntityFrameworkCore;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Repositories;
using N5.Permissions.Infrastructure.Persistence;

namespace N5.Permissions.Infrastructure.Repositories;

public class PermissionTypeRepository : IPermissionTypeRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PermissionType>> GetAllAsync()
    {
        return await _context.PermissionTypes.ToListAsync();
    }

    public async Task<PermissionType?> GetByIdAsync(int id)
    {
        return await _context.PermissionTypes.FindAsync(id);
    }

    public async Task AddAsync(PermissionType permissionType)
    {
        await _context.PermissionTypes.AddAsync(permissionType);
    }

    public async Task UpdateAsync(PermissionType permissionType)
    {
        _context.PermissionTypes.Update(permissionType);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var permissionType = await _context.PermissionTypes.FindAsync(id);
        if (permissionType != null)
        {
            _context.PermissionTypes.Remove(permissionType);
        }
    }
}
