using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entities = Product.Core.Entities;

namespace Product.Core.Abstract
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync(params string[] includes);
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate, params string[] includes);
        Task<T> GetAsync(int id, params string[] includes);
    }
}
