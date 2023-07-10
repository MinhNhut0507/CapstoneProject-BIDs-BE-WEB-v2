using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.NotificationModule.Interface
{
    public interface INotificationRepository : IRepository<Notification>
    {
        public Task<ICollection<Notification>> GetNotificationsBy(
               Expression<Func<Notification, bool>> filter = null,
               Func<IQueryable<Notification>, ICollection<Notification>> options = null,
               string includeProperties = null
           );
    }
}
