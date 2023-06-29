using Business_Logic.Modules.CategoryModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CategoryModule
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly BIDsContext _db;

        public CategoryRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }


        public async Task<ICollection<Category>> GetCategorysBy(
            Expression<Func<Category, bool>> filter = null,
            Func<IQueryable<Category>, ICollection<Category>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Category> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            query = query.Include(c => c.Descriptions.ToList());

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
