using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Domain.Repositories;

public interface IPermissionRepository
{
    Task<Permission?> GetByIdAsync(int id);
    Task<IEnumerable<Permission>> GetAllAsync();
    Task<Permission> AddAsync(Permission permission);
    Task UpdateAsync(Permission permission);
    Task<bool> ExistsAsync(int id);
}