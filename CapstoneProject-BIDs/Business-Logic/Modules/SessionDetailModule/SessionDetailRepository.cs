using Business_Logic.Modules.SessionDetailModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionDetailModule
{
    public class SessionDetailRepository : Repository<SessionDetail>, ISessionDetailRepository
    {
        private readonly BIDsContext _db;

        public SessionDetailRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<SessionDetail>> GetSessionDetailsBy(
            Expression<Func<SessionDetail, bool>> filter = null,
            Func<IQueryable<SessionDetail>, ICollection<SessionDetail>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<SessionDetail> query = DbSet;

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
            query = query.Include(s => s.Session);
            query = query.Include(s => s.Session.Item);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
