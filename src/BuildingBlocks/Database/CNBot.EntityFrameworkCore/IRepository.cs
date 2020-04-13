using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CNBot.EntityFrameworkCore
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        EntityEntry<TEntity> Entry([NotNull] TEntity entity);
        Task<TEntity> GetByIdAsync(object id);
        Task AddAsync(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(IEnumerable<TEntity> entities);
        IQueryable<TEntity> TableNoTracking { get; }
    }
}
