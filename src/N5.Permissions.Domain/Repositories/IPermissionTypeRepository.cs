using N5.Permissions.Domain.Entities;

namespace N5.Permissions.Domain.Repositories;

public interface IPermissionTypeRepository
{
    Task<IEnumerable<PermissionType>> GetAllAsync();
    Task<PermissionType?> GetByIdAsync(int id);
    Task AddAsync(PermissionType permissionType);
    Task UpdateAsync(PermissionType permissionType);
    Task DeleteAsync(int id);
}
