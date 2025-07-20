using SD_Turizm.Core.Entities;

namespace SD_Turizm.Application.Services
{
    public interface IGenericService<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
} 