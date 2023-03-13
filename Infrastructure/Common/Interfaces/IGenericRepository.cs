using System.Linq.Expressions;

namespace Ifrastructure.Common.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Get();
        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        public TEntity FindById(int id);
        public Task Create(TEntity item);
        public Task CreateRange(IEnumerable<TEntity> items);        
        public Task Remove(int id);
        public Task Remove(TEntity item);
        public Task RemoveRange(IEnumerable<TEntity> items);
        public Task Update(TEntity item);
        public IQueryable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        public TEntity GetWithIncludeById(Func<TEntity, bool> id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
