using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class StaffHub:Hub
    {
        public async Task StaffAdd(Staff staff)
        {
            await Clients.All.SendAsync("ReceiveStaffAdd", staff);
        }

        public async Task StaffUpdate(Staff staff)
        {
            await Clients.All.SendAsync("ReceiveStaffUpdate", staff);
        }

        public async Task StaffDelete(Staff staff)
        {
            await Clients.All.SendAsync("ReceiveStaffDelete", staff);
        }
    }
}
