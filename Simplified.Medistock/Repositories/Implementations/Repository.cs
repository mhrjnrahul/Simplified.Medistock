using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Data;
using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Simplified.Medistock.Repositories.Implementations
{
    //we implement the IRepository interface for any entity type T
    public class Repository<T> : IRepository<T> where T : class
    {
        //store the context and the DbSet for the entity type
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet; // e.g. _context.Products for Product repository

        //constructor takes the context via dependency injection
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>(); // get the DbSet for the entity type
        }

        //implement the methods defined in the interface
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            // Check if entity supports soft delete (inherits BaseEntity)
            if (entity is BaseEntity baseEntity)
            {
                // Soft delete — just flag it, don't remove from DB
                baseEntity.IsDeleted = true;
                _dbSet.Update(entity);
            }
            else
            {
                // Hard delete — for entities like SystemLog that don't inherit BaseEntity
                _dbSet.Remove(entity);
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}
