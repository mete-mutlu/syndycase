using Microsoft.EntityFrameworkCore;
using Product.Core.Abstract;
using Product.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.EntityFramework.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ProductContext context;
        private DbSet<T> dbSet;

        public Repository(ProductContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public void Create(T entity)
        {
            dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;

        }

        public void Delete(T entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);

        }

        public async Task<IEnumerable<T>> GetAllAsync(params string[] includes)
        {
            var query = dbSet.AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            var query = dbSet.AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);

            return await query.Where(predicate).ToListAsync();
        }

        public async Task<T> GetAsync(int id, params string[] includes)
        {
            var query = dbSet.AsQueryable();
            foreach (var include in includes)
                query = query.Include(include);

            return await query.Where(p=>p.Id == id).FirstOrDefaultAsync();
        }

        

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

       
    }
}
