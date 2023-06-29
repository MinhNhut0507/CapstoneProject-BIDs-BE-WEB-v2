using Business_Logic.Modules.DescriptionModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.DescriptionModule
{
    public class DescriptionRepository : Repository<Description>, IDescriptionRepository
    {
        private readonly BIDsContext _db;

        public DescriptionRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Description>> GetDescriptionsBy(
            Expression<Func<Description, bool>> filter = null,
            Func<IQueryable<Description>, ICollection<Description>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Description> query = DbSet;

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

            query = query.Include(d => d.Category);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
