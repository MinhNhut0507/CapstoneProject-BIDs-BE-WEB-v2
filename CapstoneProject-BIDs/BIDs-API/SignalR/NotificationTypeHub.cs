using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class NotificationTypeHub:Hub
    {
        public async Task NotificationTypeAdd(NotificationType NotificationType)
        {
            await Clients.All.SendAsync("ReceiveNotificationTypeAdd", NotificationType);
        }

        public async Task NotificationTypeUpdate(NotificationType NotificationType)
        {
            await Clients.All.SendAsync("ReceiveNotificationTypeUpdate", NotificationType);
        }

        public async Task NotificationTypeDelete(NotificationType NotificationType)
        {
            await Clients.All.SendAsync("ReceiveNotificationTypeDelete", NotificationType);
        }
    }
}
