using Common.Utils.Repository.Interface;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserNotificationDetailModule.Interface
{
    public interface IUserNotificationDetailRepository : IRepository<UserNotificationDetail>
    {
        public Task<ICollection<UserNotificationDetail>> GetUserNotificationDetailsBy(
               Expression<Func<UserNotificationDetail, bool>> filter = null,
               Func<IQueryable<UserNotificationDetail>, ICollection<UserNotificationDetail>> options = null,
               string includeProperties = null
           );
    }
}
