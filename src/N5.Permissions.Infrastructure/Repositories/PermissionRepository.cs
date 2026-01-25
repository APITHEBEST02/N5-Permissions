using Microsoft.EntityFrameworkCore;
using N5.Permissions.Domain.Entities;
using N5.Permissions.Domain.Repositories;
using N5.Permissions.Infrastructure.Persistence;

namespace N5.Permissions.Infrastructure.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _context;

    public PermissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Permission?> GetByIdAsync(int id)
    {
        return await _context.Permissions
            .Include(p => p.PermissionType)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Permission>> GetAllAsync()
    {
        return await _context.Permissions
            .Include(p => p.PermissionType)
            .ToListAsync();
    }

    public async Task<Permission> AddAsync(Permission permission)
    {
        await _context.Permissions.AddAsync(permission);
        return permission;
    }

    public async Task UpdateAsync(Permission permission)
    {
        _context.Permissions.Update(permission);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Permissions.AnyAsync(p => p.Id == id);
    }
}