using Business_Logic.Modules.BanHistoryModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BanHistoryModule
{
    public class BanHistoryRepository : Repository<BanHistory>, IBanHistoryRepository
    {
        private readonly BIDsContext _db;

        public BanHistoryRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<BanHistory>> GetBanHistorysBy(
            Expression<Func<BanHistory, bool>> filter = null,
            Func<IQueryable<BanHistory>, ICollection<BanHistory>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<BanHistory> query = DbSet;

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

            query = query.Include(s => s.User);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
