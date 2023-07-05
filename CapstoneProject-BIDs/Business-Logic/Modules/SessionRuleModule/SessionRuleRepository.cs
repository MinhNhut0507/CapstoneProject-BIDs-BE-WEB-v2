using Business_Logic.Modules.SessionRuleModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionRuleModule
{
    public class SessionRuleRepository : Repository<SessionRule>, ISessionRuleRepository
    {
        private readonly BIDsContext _db;

        public SessionRuleRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<SessionRule>> GetSessionRulesBy(
            Expression<Func<SessionRule, bool>> filter = null,
            Func<IQueryable<SessionRule>, ICollection<SessionRule>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<SessionRule> query = DbSet;

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
