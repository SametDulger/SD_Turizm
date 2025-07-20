using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;

namespace SD_Turizm.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;

        public GenericService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _unitOfWork.Repository<T>().GetAllAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<T>().GetByIdAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _unitOfWork.Repository<T>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await _unitOfWork.Repository<T>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _unitOfWork.Repository<T>().DeleteAsync(entity.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 