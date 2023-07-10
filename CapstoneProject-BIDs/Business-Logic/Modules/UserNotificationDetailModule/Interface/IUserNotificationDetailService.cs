using Business_Logic.Modules.UserNotificationDetailModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserNotificationDetailModule.Interface
{
    public interface IUserNotificationDetailService
    {
        public Task<UserNotificationDetail> AddNewUserNotificationDetail(CreateUserNotificationDetailRequest UserNotificationDetailCreate);

        //public Task<UserNotificationDetail> UpdateUserNotificationDetail(UpdateUserNotificationDetailRequest UserNotificationDetailUpdate);

        public Task<ICollection<UserNotificationDetail>> GetAll();

        public Task<ICollection<UserNotificationDetail>> GetUserNotificationDetailByUser(Guid id);

    }
}
