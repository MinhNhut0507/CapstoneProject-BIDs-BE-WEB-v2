using Business_Logic.Modules.FeeModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.FeeModule
{
    public class FeeRepository : Repository<Fee>, IFeeRepository
    {
        private readonly BIDsContext _db;

        public FeeRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Fee>> GetFeesBy(
            Expression<Func<Fee, bool>> filter = null,
            Func<IQueryable<Fee>, ICollection<Fee>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Fee> query = DbSet;

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

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
