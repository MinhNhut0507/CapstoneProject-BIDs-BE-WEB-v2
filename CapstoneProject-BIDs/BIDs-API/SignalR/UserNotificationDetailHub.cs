using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class UserNotificationDetailHub:Hub
    {
        public async Task UserNotificationDetailAdd(UserNotificationDetail UserNotificationDetail)
        {
            await Clients.All.SendAsync("ReceiveUserNotificationDetailAdd", UserNotificationDetail);
        }

        public async Task UserNotificationDetailUpdate(UserNotificationDetail UserNotificationDetail)
        {
            await Clients.All.SendAsync("ReceiveUserNotificationDetailUpdate", UserNotificationDetail);
        }

        public async Task UserNotificationDetailDelete(UserNotificationDetail UserNotificationDetail)
        {
            await Clients.All.SendAsync("ReceiveUserNotificationDetailDelete", UserNotificationDetail);
        }
    }
}
