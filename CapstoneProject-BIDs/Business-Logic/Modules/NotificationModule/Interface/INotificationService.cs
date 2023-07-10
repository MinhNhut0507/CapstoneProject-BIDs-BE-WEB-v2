using Business_Logic.Modules.NotificationModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.NotificationModule.Interface
{
    public interface INotificationService
    {
        public Task<Notification> AddNewNotification(CreateNotificationRequest NotificationCreate);

        public Task<Notification> UpdateNotification(UpdateNotificationRequest NotificationUpdate);

        public Task<Notification> DeleteNotification(Guid NotificationDeleteID);

        public Task<ICollection<Notification>> GetAll();

    }
}
