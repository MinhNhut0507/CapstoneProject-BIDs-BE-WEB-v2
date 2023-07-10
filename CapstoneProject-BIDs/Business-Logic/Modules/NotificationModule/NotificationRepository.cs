using Business_Logic.Modules.NotificationModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.NotificationModule
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly BIDsContext _db;

        public NotificationRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<Notification>> GetNotificationsBy(
            Expression<Func<Notification, bool>> filter = null,
            Func<IQueryable<Notification>, ICollection<Notification>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<Notification> query = DbSet;

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
