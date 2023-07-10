using Business_Logic.Modules.NotificationTypeModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.NotificationTypeModule.Interface
{
    public interface INotificationTypeService
    {
        public Task<NotificationType> AddNewNotificationType(CreateNotificationTypeRequest NotificationTypeCreate);

        public Task<NotificationType> UpdateNotificationType(UpdateNotificationTypeRequest NotificationTypeUpdate);

        public Task<ICollection<NotificationType>> GetAll();

    }
}
