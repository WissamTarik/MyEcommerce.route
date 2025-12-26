using MyEccommerce.Route.Domain.Contracts;
using MyEcommerce.Route.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Specification
{
    public class BaseSpecification<TKey, TEntity> : ISpecification<TKey, TEntity> where TEntity:BaseEntity<TKey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, bool>>? Criteria { get ; set; }
        public Expression<Func<TEntity, object>>? OrderBy { get ; set ; }
        public Expression<Func<TEntity, object>>? OrderByDescendingly { get ; set ; }
        public int Take { get ; set; }
        public int Skip { get; set; }
        public bool IsPagination { get ; set; }

        public BaseSpecification(Expression<Func<TEntity,bool>> expression)
        {
            Criteria = expression;
        }
        public void ApplyOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }
        public void ApplyOrderByDescendingly(Expression<Func<TEntity, object>> expression)
        {
            OrderByDescendingly = expression;
        }
        public void ApplyPagination(int pageIndex,int pageSize)
        {
            //  3   5
            // take=5
            //skip= (pageindex-1)*pagesize
            IsPagination = true;
            Skip = (pageIndex - 1) * pageSize;
            Take=pageSize;
        }
    }
}
