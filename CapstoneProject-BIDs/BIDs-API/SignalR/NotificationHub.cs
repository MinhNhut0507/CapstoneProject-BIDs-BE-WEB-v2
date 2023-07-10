using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class NotificationHub:Hub
    {
        public async Task NotificationAdd(Notification Notification)
        {
            await Clients.All.SendAsync("ReceiveNotificationAdd", Notification);
        }

        public async Task NotificationUpdate(Notification Notification)
        {
            await Clients.All.SendAsync("ReceiveNotificationUpdate", Notification);
        }

        public async Task NotificationDelete(Notification Notification)
        {
            await Clients.All.SendAsync("ReceiveNotificationDelete", Notification);
        }
    }
}
