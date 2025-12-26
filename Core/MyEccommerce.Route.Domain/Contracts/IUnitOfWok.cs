using MyEcommerce.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Domain.Contracts
{
    public interface IUnitOfWok
    {
        IGenericRepository<TKey,TEntity> GetRepository<TKey,TEntity>() where TEntity:BaseEntity<TKey>;

        Task SaveChangeAsync();
    }
}
