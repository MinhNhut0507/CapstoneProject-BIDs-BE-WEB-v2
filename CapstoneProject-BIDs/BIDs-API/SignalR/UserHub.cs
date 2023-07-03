using Data_Access.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BIDs_API.SignalR
{
    public class UserHub:Hub
    {
        public async Task UserAdd(Users user)
        {
            await Clients.All.SendAsync("ReceiveUserAdd", user);
        }

        public async Task UserUpdate(Users user)
        {
            await Clients.All.SendAsync("ReceiveUserUpdate", user);
        }

        public async Task UserActive(Users user)
        {
            await Clients.All.SendAsync("ReceiveUserActive", user);
        }

        public async Task UserBan(Users user)
        {
            await Clients.All.SendAsync("ReceiveUserBan", user);
        }

        public async Task UserUnban(Users user)
        {
            await Clients.All.SendAsync("ReceiveUserUnban", user);
        }

        public async Task UserDeny(Users user)
        {
            await Clients.All.SendAsync("ReceiveUserDeny", user);
        }
    }
}
