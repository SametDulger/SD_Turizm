using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SD_Turizm.Core.Entities;
using SD_Turizm.Core.Interfaces;
using SD_Turizm.Infrastructure.Data;

namespace SD_Turizm.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).Where(x => x.IsActive).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.IsActive = true;
            
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _dbSet.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsActive = false;
                entity.UpdatedDate = DateTime.UtcNow;
                await UpdateAsync(entity);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<T> Query()
        {
            return _dbSet.Where(x => x.IsActive).AsQueryable();
        }
    }
} 