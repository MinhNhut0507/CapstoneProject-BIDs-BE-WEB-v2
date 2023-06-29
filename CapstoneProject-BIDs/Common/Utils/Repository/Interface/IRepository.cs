using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid key);

        Task AddAsync(T entity);

        void Add(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task<ICollection<T>> GetAll(
                Func<IQueryable<T>,
                ICollection<T>> options = null,
                string includeProperties = null
        );

        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
        );

        Task UpdateAsync(T entity);

        Task UpdateRangeAsync(IEnumerable<T> entities);

        Task RemoveAsync(string key);

        Task RemoveAsync(T entity);

        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
