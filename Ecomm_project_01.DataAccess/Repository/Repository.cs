 using Ecomm_project_01.DataAccess.Data;
using Ecomm_project_01.DataAccess.Repository.iRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_project_01.DataAccess.Repository
{
    public class Repository<T> : iRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();
        }

        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T FirstOrDeafault(Expression<Func<T, bool>> filter = null,
            string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
                query = query.Where(filter);
            if(includeProperties != null)
            {
                foreach (var includeprop in includeProperties.Split(new[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);

                }
            }
            return query.FirstOrDefault();
        }

        public T Get(int id)
        {
            return dbset.Find(id);
            
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, 
            string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if(filter != null)
                query = query.Where(filter);
            if(includeProperties != null)
            {
                foreach (var includeprop in includeProperties.Split(new[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(includeprop);

                }
            }
            if(orderby != null)
                return orderby(query).ToList();
            return query.ToList();
        }

        public void Remove(T entity)
        {
           dbset.Remove(entity);
        }

        public void Remove(int id)
        {
            dbset.Remove(Get(id));
        }

        public void RemoveRange(IEnumerable<T> values)
        {
            dbset.RemoveRange(values);
        }

        public void Update(T entity)
        {
            _context.ChangeTracker.Clear();
            dbset.Update(entity);
        }
    }
}
