using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Core.DTOs;

namespace SD_Turizm.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Permission>().GetAllAsync();
        }

        public async Task<Permission?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Permission>().GetByIdAsync(id);
        }

        public async Task<Permission?> GetByNameAsync(string name)
        {
            var permissions = await _unitOfWork.Repository<Permission>().FindAsync(p => p.Name == name);
            return permissions.FirstOrDefault();
        }

        public async Task<Permission> CreateAsync(Permission permission)
        {
            if (await PermissionNameExistsAsync(permission.Name))
                throw new InvalidOperationException("Bu yetki adı zaten kullanılıyor.");

            permission.CreatedDate = DateTime.UtcNow;
            permission.IsActive = true;

            await _unitOfWork.Repository<Permission>().AddAsync(permission);
            await _unitOfWork.SaveChangesAsync();
            return permission;
        }

        public async Task<Permission> UpdateAsync(Permission permission)
        {
            var existingPermission = await GetByIdAsync(permission.Id);
            if (existingPermission == null)
                throw new InvalidOperationException("Yetki bulunamadı.");

            if (permission.Name != existingPermission.Name && await PermissionNameExistsAsync(permission.Name))
                throw new InvalidOperationException("Bu yetki adı zaten kullanılıyor.");

            permission.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.Repository<Permission>().UpdateAsync(permission);
            await _unitOfWork.SaveChangesAsync();
            return permission;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Repository<Permission>().DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> PermissionExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Permission>().ExistsAsync(id);
        }

        public async Task<bool> PermissionNameExistsAsync(string name)
        {
            var permissions = await _unitOfWork.Repository<Permission>().FindAsync(p => p.Name == name);
            return permissions.Any();
        }

        public async Task<IEnumerable<Permission>> GetByResourceAsync(string resource)
        {
            return await _unitOfWork.Repository<Permission>().FindAsync(p => p.Resource == resource);
        }

        public async Task<IEnumerable<Permission>> GetByActionAsync(string action)
        {
            return await _unitOfWork.Repository<Permission>().FindAsync(p => p.Action == action);
        }

        public async Task<PagedResult<Permission>> GetPagedAsync(int page, int pageSize, string? searchTerm = null)
        {
            var query = _unitOfWork.Repository<Permission>().GetAllAsync().Result.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => 
                    p.Name.Contains(searchTerm) || 
                    (p.Description != null && p.Description.Contains(searchTerm)) ||
                    p.Resource.Contains(searchTerm) ||
                    p.Action.Contains(searchTerm));
            }

            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return await Task.FromResult(new PagedResult<Permission>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        public async Task<IEnumerable<string>> GetAllResourcesAsync()
        {
            var permissions = await GetAllAsync();
            return permissions.Select(p => p.Resource).Distinct().OrderBy(r => r);
        }

        public async Task<IEnumerable<string>> GetAllActionsAsync()
        {
            var permissions = await GetAllAsync();
            return permissions.Select(p => p.Action).Distinct().OrderBy(a => a);
        }
    }
}
