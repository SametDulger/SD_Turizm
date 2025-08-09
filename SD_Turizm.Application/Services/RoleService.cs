using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Role>().GetAllAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Role>().GetByIdAsync(id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            var roles = await _unitOfWork.Repository<Role>().FindAsync(r => r.Name == name);
            return roles.FirstOrDefault();
        }

        public async Task<Role> CreateAsync(Role role)
        {
            if (await RoleNameExistsAsync(role.Name))
                throw new InvalidOperationException("Bu rol adı zaten kullanılıyor.");

            role.CreatedDate = DateTime.UtcNow;
            role.IsActive = true;

            await _unitOfWork.Repository<Role>().AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
            return role;
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var existingRole = await GetByIdAsync(role.Id);
            if (existingRole == null)
                throw new InvalidOperationException("Rol bulunamadı.");

            if (role.Name != existingRole.Name && await RoleNameExistsAsync(role.Name))
                throw new InvalidOperationException("Bu rol adı zaten kullanılıyor.");

            role.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<Role>().UpdateAsync(role);
            await _unitOfWork.SaveChangesAsync();
            return role;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Role>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> RoleExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Role>().ExistsAsync(id);
        }

        public async Task<bool> RoleNameExistsAsync(string name)
        {
            var roles = await _unitOfWork.Repository<Role>().FindAsync(r => r.Name == name);
            return roles.Any();
        }

        public async Task<IEnumerable<Permission>> GetRolePermissionsAsync(int roleId)
        {
            var rolePermissions = await _unitOfWork.Repository<RolePermission>().FindAsync(rp => rp.RoleId == roleId);
            var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();
            
            if (!permissionIds.Any()) return new List<Permission>();

            var permissions = await _unitOfWork.Repository<Permission>().FindAsync(p => permissionIds.Contains(p.Id));
            return permissions;
        }

        public async Task<bool> AssignPermissionAsync(int roleId, int permissionId)
        {
            // Check if role and permission exist
            var role = await GetByIdAsync(roleId);
            var permission = await _unitOfWork.Repository<Permission>().GetByIdAsync(permissionId);

            if (role == null || permission == null) return false;

            // Check if permission is already assigned
            var existingRolePermission = await _unitOfWork.Repository<RolePermission>().FindAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            if (existingRolePermission.Any()) return true; // Already assigned

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Repository<RolePermission>().AddAsync(rolePermission);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionAsync(int roleId, int permissionId)
        {
            var rolePermissions = await _unitOfWork.Repository<RolePermission>().FindAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            var rolePermission = rolePermissions.FirstOrDefault();

            if (rolePermission == null) return false;

            await _unitOfWork.Repository<RolePermission>().DeleteAsync(rolePermission.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasPermissionAsync(int roleId, string resource, string action)
        {
            var permissions = await GetRolePermissionsAsync(roleId);
            return permissions.Any(p => p.Resource == resource && p.Action == action);
        }

        public async Task<PagedResult<Role>> GetPagedAsync(int page, int pageSize, string? searchTerm = null)
        {
            var query = _unitOfWork.Repository<Role>().GetAllAsync().Result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(r => 
                    r.Name.Contains(searchTerm) || 
                    r.Description.Contains(searchTerm));
            }

            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(new PagedResult<Role>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }
    }
}
