using Business_Logic.Modules.UserNotificationDetailModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserNotificationDetailModule
{
    public class UserNotificationDetailRepository : Repository<UserNotificationDetail>, IUserNotificationDetailRepository
    {
        private readonly BIDsContext _db;

        public UserNotificationDetailRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<UserNotificationDetail>> GetUserNotificationDetailsBy(
            Expression<Func<UserNotificationDetail, bool>> filter = null,
            Func<IQueryable<UserNotificationDetail>, ICollection<UserNotificationDetail>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<UserNotificationDetail> query = DbSet;

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

            query = query.Include(x => x.User)
                .Include(x => x.Type);

            return options != null ? options(query).ToList() : await query.ToListAsync();
        }
    }
}
