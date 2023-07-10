using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.NotificationTypeModule.Interface
{
    public interface INotificationTypeRepository : IRepository<NotificationType>
    {
        public Task<ICollection<NotificationType>> GetNotificationTypesBy(
               Expression<Func<NotificationType, bool>> filter = null,
               Func<IQueryable<NotificationType>, ICollection<NotificationType>> options = null,
               string includeProperties = null
           );
    }
}
