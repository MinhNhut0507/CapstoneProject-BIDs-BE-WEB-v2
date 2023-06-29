using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class AdminHub:Hub
    {
        public async Task AdminAdd(Admin Admin)
        {
            await Clients.All.SendAsync("ReceiveAdminAdd", Admin);
        }

        public async Task AdminUpdate(Admin Admin)
        {
            await Clients.All.SendAsync("ReceiveAdminUpdate", Admin);
        }

        public async Task AdminDelete(Admin Admin)
        {
            await Clients.All.SendAsync("ReceiveAdminDelete", Admin);
        }
    }
}
