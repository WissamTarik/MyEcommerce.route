using Microsoft.EntityFrameworkCore;
using MyEccommerce.Route.Domain.Contracts;
using MyEcommerce.Route.Domain.Entities;
using MyEcommerce.Route.Domain.Entities.Products;
using MyEcommerce.Route.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence.Repositories
{
    public class GenericRepository<TKey, TEntity> (MyEcommerceDbContext _context): IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(Product))
            {
                return (IEnumerable<TEntity>) await _context.Products.Include(p=>p.ProductBrand).Include(p=>p.ProductType).ToListAsync();
            }
            else
            {
               return await _context.Set<TEntity>().ToListAsync();
            }
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            if (typeof(TEntity) == typeof(Product))
            {
             return  await  _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).FirstOrDefaultAsync(p => p.Id.Equals(id)) as TEntity;
            }
            else
            {
                return await _context.Set<TEntity>().FindAsync(id);
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }
        public  void Update(TEntity entity)
        {
             _context.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TKey, TEntity> spec)
        {
           
           var query=ApplySpecification(spec);
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(ISpecification<TKey, TEntity> spec)
        {

            var query = await ApplySpecification(spec).FirstOrDefaultAsync();
            return query;
        }
        private IQueryable<TEntity> ApplySpecification(ISpecification<TKey,TEntity> spec)
        {
            return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec);
        }

        public async Task<int> CountAsync(ISpecification<TKey, TEntity> spec)
        {
           return await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec).CountAsync();
        }

        public async Task<decimal> AverageAsync(ISpecification<TKey, TEntity> spec,Expression<Func<TEntity,decimal>> selector)
        {
            return await SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), spec).AverageAsync(selector);
        }
    }
}
