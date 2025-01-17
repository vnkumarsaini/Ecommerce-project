﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.DataAccess.Repository.iRepository
{
    public interface iRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void Remove(int id);
        void RemoveRange(IEnumerable<T> values);
        T Get(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter=null,
            Func<IQueryable <T>,IOrderedQueryable <T>> orderby = null,
            String includeProperties = null
            );
        T FirstOrDeafault(
             Expression<Func<T, bool>> filter = null,
             String includeProperties = null
            );
      
    }
}
