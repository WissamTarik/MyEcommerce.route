using MyEcommerce.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Contracts
{
    public interface IGenericRepository<TKey,TEntity> where TEntity : BaseEntity<TKey>
    {

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TKey,TEntity> spec);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> GetByIdAsync(ISpecification<TKey,TEntity> spec);
        Task<int> CountAsync(ISpecification<TKey,TEntity> spec);
        Task<decimal> AverageAsync(ISpecification<TKey,TEntity> spec, Expression<Func<TEntity, decimal>> selector);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

    }
}
