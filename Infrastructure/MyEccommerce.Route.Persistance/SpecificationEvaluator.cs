using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MyEccommerce.Route.Domain.Contracts;
using MyEcommerce.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Persistence
{
    public class SpecificationEvaluator
    {
      public static IQueryable<TEntity> GetQuery<TKey,TEntity>(IQueryable<TEntity> inputQuery,ISpecification<TKey,TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            if(spec.Criteria is not null)
            {
                query=query.Where(spec.Criteria);
            }
            if(spec.OrderBy is not null)
            {
                query=query.OrderBy(spec.OrderBy);
            }
            else if(spec.OrderByDescendingly is not null)
            {
                query=query.OrderByDescending(spec.OrderByDescendingly);
            }
            if (spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includes.Aggregate(query, (query, includeExpression) =>query.Include(includeExpression));
            return query;
        }
    }
}
