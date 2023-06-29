using Business_Logic.Modules.SessionModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionModule
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly BIDsContext _db;

        public SessionRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Session>> GetSessionsBy(
            Expression<Func<Session, bool>> filter = null,
            Func<IQueryable<Session>, ICollection<Session>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Session> query = DbSet;

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

            query = query.Include(s => s.Fee);
            query = query.Include(s => s.Item);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
