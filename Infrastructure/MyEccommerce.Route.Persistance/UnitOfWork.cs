using MyEccommerce.Route.Domain.Contracts;
using MyEccommerce.Route.Persistence.Repositories;
using MyEcommerce.Route.Domain.Entities;
using MyEcommerce.Route.Persistence.Contexts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence
{
    public class UnitOfWork(MyEcommerceDbContext _context) : IUnitOfWok
    {
        public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : BaseEntity<TKey>
        {
            ConcurrentDictionary<string, object> GetRepo = new ConcurrentDictionary<string, object>();
            var Repo = GetRepo.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TKey, TEntity>(_context));
            return (IGenericRepository<TKey,TEntity>)Repo;
        }

        public async Task SaveChangeAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
