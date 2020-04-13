using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.EntityFrameworkCore
{
    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private DbSet<TEntity> _entities;
        private readonly IDbContext _context;
        public EfRepository(IDbContext context)
        {
            _context = context;
        }
        public DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = this._context.Set<TEntity>();
                }
                return _entities;
            }
        }
        public IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public async Task AddAsync(TEntity entity)
        {
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<TEntity> entities)
        {
            Entities.AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            Entities.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            Entities.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public EntityEntry<TEntity> Entry([NotNull] TEntity entity)
        {
            return _context.Entry(entity);
        }
    }
}
