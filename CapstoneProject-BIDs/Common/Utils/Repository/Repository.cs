using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BIDsContext _db;
        public readonly DbSet<T> DbSet;

        public Repository(BIDsContext db)
        {
            _db = db;
            DbSet = _db.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
            _db.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
            await _db.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(Guid key)
        {
            return await DbSet.FindAsync(key);
        }

        public async Task<ICollection<T>> GetAll(Func<IQueryable<T>, ICollection<T>> options = null, string includeProperties = null)
        {
            try
            {
                IQueryable<T> query = DbSet;

                if (includeProperties != null)
                {
                    foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                if (options != null)
                {
                    return options(query).ToList();
                }

                return await query.ToListAsync();
            }
            catch
            {
                Console.WriteLine("Error");
            }
            return null;
        }

        public Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(string key)
        {
            var entity = await DbSet.FindAsync(key);
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            DbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            DbSet.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            DbSet.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            DbSet.UpdateRange(entities);
            await _db.SaveChangesAsync();
        }
    }
}
