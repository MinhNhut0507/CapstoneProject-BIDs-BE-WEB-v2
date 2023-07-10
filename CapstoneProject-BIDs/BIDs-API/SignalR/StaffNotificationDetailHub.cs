using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class StaffNotificationDetailHub:Hub
    {
        public async Task StaffNotificationDetailAdd(StaffNotificationDetail StaffNotificationDetail)
        {
            await Clients.All.SendAsync("ReceiveStaffNotificationDetailAdd", StaffNotificationDetail);
        }

        public async Task StaffNotificationDetailUpdate(StaffNotificationDetail StaffNotificationDetail)
        {
            await Clients.All.SendAsync("ReceiveStaffNotificationDetailUpdate", StaffNotificationDetail);
        }

        public async Task StaffNotificationDetailDelete(StaffNotificationDetail StaffNotificationDetail)
        {
            await Clients.All.SendAsync("ReceiveStaffNotificationDetailDelete", StaffNotificationDetail);
        }
    }
}
