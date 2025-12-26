using MyEccommerce.Route.Domain.Entities.Identity;
using MyEccommerce.Route.Shared.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEccommerce.Route.Services.Specification
{
    public class AdminUserSpecification(UserQueryParameters _parameters)
    {

      
        public IQueryable<AppUser> AllApplies(IQueryable<AppUser> query)
        {
           query= ApplySearch(query);
            query = ApplySort(query);
            query = ApplyPagination(query);
            return query;
        }
        
        private IQueryable<AppUser> ApplySearch(IQueryable<AppUser> query)
        {
            if (!string.IsNullOrEmpty(_parameters.Search))
            {
                query=query.Where(q=>q.Email.ToLower().Contains(_parameters.Search.ToLower()));
            }
            return query;
        }
        private IQueryable<AppUser> ApplySort(IQueryable<AppUser> query)
        {

            if (string.IsNullOrEmpty(_parameters.Sort))
            {
                query=query.OrderBy(q=>q.Id);
            }
            else
            {
                query = _parameters.Sort switch
                {
                    "emailasc" => query.OrderBy(q => q.Email),
                    "emaildesc" => query.OrderByDescending(q => q.Email),
                    "name" => query.OrderBy(q => q.UserName),
                    _ => query.OrderBy(q => q.UserName)
                };
            }
            return query;
        }
        private IQueryable<AppUser> ApplyPagination(IQueryable<AppUser> query)
        {

           return query.Skip((_parameters.PageIndex - 1) * _parameters.PageSize).Take(_parameters.PageSize);
        }

        
    }
}
