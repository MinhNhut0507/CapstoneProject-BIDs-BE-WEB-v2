using Business_Logic.Modules.NotificationTypeModule.Interface;
using Common.Utils.Repository;
using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.NotificationTypeModule
{
    public class NotificationTypeRepository : Repository<NotificationType>, INotificationTypeRepository
    {
        private readonly BIDsContext _db;

        public NotificationTypeRepository(BIDsContext db) : base(db)
        {
            _db = db;
        }

        public async Task<ICollection<NotificationType>> GetNotificationTypesBy(
            Expression<Func<NotificationType, bool>> filter = null,
            Func<IQueryable<NotificationType>, ICollection<NotificationType>> options = null,
            string includeProperties = null
        )
        {
            IQueryable<NotificationType> query = DbSet;

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
